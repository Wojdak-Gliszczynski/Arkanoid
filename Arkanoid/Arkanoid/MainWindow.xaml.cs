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
        private List<Rect> walls;
        //-------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += MainLoopFunction;
            CreateWalls();
            GameControl.StartGame(ref programGrid, ref platform, ref balls, ref bricks, ref bonuses);
        }
        //-------------------------------------------------------
        private void CreateWalls()
        {
            //Physical part
            walls = new List<Rect>();

            Rect leftWall = new Rect(0, 0, 160, 600);
            Rect rightWall = new Rect(640, 0, 160, 600);
            Rect ceiling = new Rect(0, -4, 800, 4);

            walls.Add(leftWall);
            walls.Add(rightWall);
            walls.Add(ceiling);

            //Graphics layout
            TransformingImage leftWallImg = new TransformingImage
                (new Uri("./Graphics/background_wall.png", UriKind.Relative), programGrid, 0, 0, 160, 600);
            TransformingImage rightWallImg = new TransformingImage
                (new Uri("./Graphics/background_wall.png", UriKind.Relative), programGrid, 640, 0, 160, 600);
        }

        private void MainLoopFunction(Object Sender, EventArgs e)
        {
            GameControl.CheckGameState(ref programGrid, ref platform, ref balls, ref bricks, ref bonuses);
            GameControl.RefreshStatistics(ref programGrid);
            DoElementsActions();
        }

        private void RemoveElement<T>(List<T> listOfElements, T element) where T : UIElement
        {
            programGrid.Children.Remove(element);
            listOfElements.Remove(element);
        }

        //ACTIONS PERFORMED ON GAME ELEMENTS 
        private void BallsActions()
        {
            List<Ball> destroyedBalls = new List<Ball>();
            foreach (Ball ball in balls)
            {
                ball.Move();

                foreach (Rect wall in walls)
                    if (ball.HasCollisionWith(wall))
                        ball.Bounce(wall);

                if (ball.IsDestroyed())
                    destroyedBalls.Add(ball);
            }
            foreach (Ball ball in destroyedBalls)
                balls.Remove(ball);
        }
        private void BricksActions()
        {
            List<Brick> destroyedBricks = new List<Brick>();
            foreach (Brick brick in bricks)
            {
                if (!destroyedBricks.Contains(brick))
                    brick.Collisions(programGrid, balls, bricks, destroyedBricks);
            }
            foreach (Brick brick in destroyedBricks)
            {
                if (Bonus.OrCreate())
                    bonuses.Add(new Bonus(programGrid, Bonus.Random(), brick.Margin.Left, brick.Margin.Top));
                RemoveElement<Brick>(bricks, brick);
            }
        }
        private void BonusesActions()
        {
            foreach (Bonus bonus in bonuses)
                bonus.Move();
        }
        private void PlatformActions()
        {
            platform.Control(Mouse.GetPosition(this));
            platform.Collisions(ref programGrid, ref balls, ref bonuses);
        }
        private void DoElementsActions()
        {
            BallsActions();
            BricksActions();
            BonusesActions();
            PlatformActions();
        }
    }
}
