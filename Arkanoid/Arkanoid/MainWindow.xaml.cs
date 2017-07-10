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
        private List<Bonus> bonuses;
        private List<Explosion> explosions;
        private Rect leftWall, rightWall, ceiling;
        //private GameControl stats;
        
        public MainWindow()
        {
            InitializeComponent();
            //Timer
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            //Ścianki
            leftWall = new Rect(0, 0, 160, 600);
            rightWall = new Rect(640, 0, 160, 600);
            ceiling = new Rect(0, 0, 800, 16);

            BitmapImage bmp = new BitmapImage(new Uri("./Graphics/background_wall.png", UriKind.Relative));

            Image leftWallImg = new Image();
            leftWallImg.Margin = new Thickness(0, 0, 0, 0);
            leftWallImg.HorizontalAlignment = HorizontalAlignment.Left;
            leftWallImg.VerticalAlignment = VerticalAlignment.Top;
            leftWallImg.Source = bmp;
            grid1.Children.Add(leftWallImg);

            Image rightWallImg = new Image();
            rightWallImg.Margin = new Thickness(640, 0, 0, 0);
            rightWallImg.HorizontalAlignment = HorizontalAlignment.Left;
            rightWallImg.VerticalAlignment = VerticalAlignment.Top;
            rightWallImg.Source = bmp;
            grid1.Children.Add(rightWallImg);

            //Start
            GameControl.StartGame(ref grid1, ref platform, ref balls, ref bricks, ref bonuses, ref explosions);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            GameControl.CheckGameState(ref grid1, ref platform, ref balls, ref bricks, ref bonuses, ref explosions);
            GameControl.RefreshStatistics(ref grid1);

            for (int i = balls.Count - 1; i >= 0; i--)
            {
                balls[i].Move();
                if (balls[i].IsDestroyed())
                {
                    grid1.Children.Remove(balls[i]);
                    balls.Remove(balls[i]);
                }
            }

            foreach (Ball ball in balls)
            {
                if (ball.HasCollisionWith(leftWall))
                    ball.Bounce(leftWall);
                else if (ball.HasCollisionWith(rightWall))
                    ball.Bounce(rightWall);
                if (ball.HasCollisionWith(ceiling)) //Bez else, bo może mieć kolizje jednocześnie ze ścianą i sufitem
                    ball.Bounce(ceiling);
            }

            List<Brick> destroyedBricks = new List<Brick>();
            for (int i = bricks.Count - 1; i >= 0; i--)
            {
                if (!destroyedBricks.Contains(bricks[i]))
                {
                    if (!bricks[i].Collisions(ref grid1, ref balls, ref bricks, ref explosions, ref destroyedBricks))
                    {
                        if (Bonus.OrCreate())
                            bonuses.Add(new Bonus(grid1, Bonus.RandomID(), bricks[i].Margin.Left, bricks[i].Margin.Top));
                    }
                }
            }
            foreach (Brick brick in destroyedBricks)
            {
                grid1.Children.Remove(brick);
                bricks.Remove(brick);
            }

            foreach (Bonus bonus in bonuses)
                bonus.Move();

            platform.Control(Mouse.GetPosition(this));

            platform.Collisions(ref grid1, ref balls, ref bricks, ref bonuses);
        }
    }
}
