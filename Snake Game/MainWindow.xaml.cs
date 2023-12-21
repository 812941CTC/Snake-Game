using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Timer = System.Threading.Timer;

namespace Snake_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Snake,Images.Body },
            {GridValue.Food, Images.Food },
            {GridValue.Wall, Images.Wall }
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.up, 0 },
            {Direction.Right, 90},
            {Direction.down,180 },
            {Direction.Left, 270}
            };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        private List<int> highScores = new List<int>();
        private int boostSpeed = 0;
        private Random random = new Random();
        private DispatcherTimer boostTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows,cols);
            boostTimer.Interval = TimeSpan.FromSeconds(1);
            boostTimer.Tick += BoostTimer_Tick;
        }

        private void BoostTimer_Tick(object sender, EventArgs e)
        {
            boostSpeed = 0;
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch(e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left); 
                    break;
                    case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                         break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.down);
                    break;
                case Key.Space:
                    boostSpeed = (boostSpeed == 0) ? GameSettings.boostSpeed : 0;
                    boostTimer.Start();
                    break;
                case Key.B:
                    if (gridValToImage[GridValue.Snake] == Images.Body)
                    {
                        gridValToImage[GridValue.Snake] = Images.Body2;
                        Images.Chead = Images.Head2;
                        Images.CdeadBody = Images.DeadBody2;
                        Images.CdeadHead = Images.DeadHead2;
                    }
                    else
                    {
                        gridValToImage[GridValue.Snake] = Images.Body;
                        Images.Chead = Images.Head;
                        Images.CdeadBody = Images.DeadBody;
                        Images.CdeadHead = Images.DeadHead2;
                    }
                   

                   // gridValToImage[GridValue.]
                    break;
                case Key.N:
                    if (gridValToImage[GridValue.Empty] == Images.Empty)
                    {
                        gridValToImage[GridValue.Empty] = Images.Empty2;
                    }
                    else
                    {
                        gridValToImage[GridValue.Empty] = Images.Empty;
                    }

                    break;
                case Key.V:
                    if (GameSettings.WallFatality == true)
                    {
                        GameSettings.WallFatality = false;
                    }
                    else
                    {
                        GameSettings.WallFatality = true;
                    }
                    break;
                    case Key.C:
                    if(GameSettings.walls == true)
                    {
                    GameSettings.walls = false;
                    }
                    else
                    {
                        GameSettings.walls = true;
                    }
                    
                    break;
               

                    

            }
            
        }
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
               
                
              await Task.Delay(200-boostSpeed);
                                
                gameState.Move();
                Draw();
            }
        }

        
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;

            }

            if(!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols/(double)rows);
            for (int r = 0; r < rows; r++)
            {
                for(int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5,0.5)
                    };
                images[r, c] = image;
                    GameGrid.Children.Add(image);



                }
            }
            return images;
        }
        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++) 
            {
            for (int c = 0; c < cols;c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Chead;
           
            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions());

            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.CdeadHead : Images.CdeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(Math.Max(50-(i*2),10));
            }
        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            boostSpeed = 0;
            ShakeWindow(GameSettings.ShakeDuration);
            await DrawDeadSnake();
            await Task.Delay(1000);
            
            OverlayText.Text = "PRESS ANY KEY TO START \n Leaderboard:";
            UpdateLeaderboard();
            Overlay.Visibility = Visibility.Visible;
        }

        private void UpdateLeaderboard()
        {
            highScores.Add(gameState.Score);
            highScores.Sort();
            highScores.Reverse();

            if(highScores.Count > 5)
            {
                highScores.RemoveAt(5);
            }
           foreach(var score in highScores)
            {
                OverlayText.Text += $"\n {score}";
            }
        }
        private async Task ShakeWindow(int durationMs)
        {
            var oLeft = this.Left;
            var oTop = this.Top;

            var shakeTimer = new DispatcherTimer(DispatcherPriority.Send);

            shakeTimer.Tick += (sender, args) =>
            {
                this.Left = oLeft + random.Next(-10, 11);
                this.Top = oTop + random.Next(-10, 11);
            };

            shakeTimer.Interval = TimeSpan.FromMilliseconds(200);
            shakeTimer.Start();

            await Task.Delay(durationMs);
            shakeTimer.Stop();

        }

    }
}
