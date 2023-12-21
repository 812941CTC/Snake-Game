using System;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake_Game
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Empty2 = LoadImage("Empty2.png");

        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");
        public readonly static ImageSource Head2 = LoadImage("Head2.png");
        public readonly static ImageSource Body2 = LoadImage("Body2.png");
        public readonly static ImageSource DeadBody2 = LoadImage("DeadBody2.png");
        public readonly static ImageSource DeadHead2 = LoadImage("DeadHead2.png");
        public readonly static ImageSource Wall = LoadImage("Wall.png");
        public static ImageSource Chead{ get; set; } = Head;
        public static ImageSource CdeadHead { get; set; } = DeadHead;
        public static ImageSource CdeadBody { get; set; } = DeadBody;
        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative)); 
        }
    }
}
