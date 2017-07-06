using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Arkanoid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Platform platform;
        private List<Ball> balls;
        private List<Brick> bricks;
        private Rect leftWall, rightWall, ceiling;
        
        public MainWindow()
        {
            InitializeComponent();
            //Timer
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            //Obiekty rozgrywki
            platform = new Platform(grid1);

            balls = new List<Ball>();
            balls.Add(new Ball(grid1));

            bricks = new List<Brick>();
            for (int i = 0; i < 15; i++)
                bricks.Add(new Brick(ref grid1, Convert.ToUInt16(i % 10), i + 1, 1));
            
            //Ścianki
            leftWall = new Rect(0, 0, 160, 600);
            rightWall = new Rect(640, 0, 160, 600);
            ceiling = new Rect(0, 0, 800, 16);

            BitmapImage bmp = new BitmapImage(new Uri("./Graphics/background_wall.png", UriKind.Relative));

            Image leftWallImg = new Image();
            leftWallImg.Margin = new Thickness(-632, 0, 0, 0);
            leftWallImg.Source = bmp;
            grid1.Children.Add(leftWallImg);

            Image rightWallImg = new Image();
            rightWallImg.Margin = new Thickness(632, 0, 0, 0);
            rightWallImg.Source = bmp;
            grid1.Children.Add(rightWallImg);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach (Ball ball in balls)
                ball.Move();

            foreach (Ball ball in balls)
            {
                if (ball.HasCollisionWith(leftWall))
                    ball.Bounce(leftWall);
                else if (ball.HasCollisionWith(rightWall))
                    ball.Bounce(rightWall);
                if (ball.HasCollisionWith(ceiling)) //Bez else, bo może mieć kolizje jednocześnie ze ścianą i sufitem
                    ball.Bounce(ceiling);
            }

            for (int i = bricks.Count - 1; i >= 0; i--)
            {
                if (!bricks[i].Collisions(ref balls))
                {
                    grid1.Children.Remove(bricks[i]);
                    bricks.Remove(bricks[i]);
                }
            }

            platform.Control(Mouse.GetPosition(this));

            platform.Collisions(ref balls);
        }
    }
}
