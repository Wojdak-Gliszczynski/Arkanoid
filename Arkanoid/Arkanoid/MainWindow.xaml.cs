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
        private Label[] scoreboardNo;
        private Label[] scoreboardNames;
        private Label[] scoreboardScore;
        private List<Border> scoreboardBorders;
        private Button buttonScoreboardBack;

        private Platform platform;
        private List<Ball> balls;
        private List<Brick> bricks;
        private List<Bonus> bonuses;
        private List<Rect> walls;
        //-------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            ScoreboardInit();
        }
        //-------------------------------------------------------
        //MAIN MENU 
        private void ScoreboardInit()
        {
            scoreboardBorders = new List<Border>();

            CreateTextBoxCol(ref scoreboardNo, 10, Width / 2.0 - 171, Height / 2.0 - 152, 24, 24);
            for (int i = 0; i < scoreboardNo.Length; i++)
                scoreboardNo[i].Content = i + 1;

            CreateTextBoxCol(ref scoreboardNames, 10, Width / 2.0 - 148, Height / 2.0 - 152, 160, 24);
            CreateTextBoxCol(ref scoreboardScore, 10, Width / 2.0 + 11, Height / 2.0 - 152, 160, 24);

            buttonScoreboardBack = CreateButton("Back", buttonScoreboardBack_Click, Width / 2.0 - 50, Height / 2.0 + 120, 100, 32);
            menuGrid.Children.Add(buttonScoreboardBack);

            HideTheScoreboard();
        }
        //Show/Hide Actions 
        //+Main Menu - Show/Hide 
        private void ShowTheMainMenu()
        {
            buttonStartGame.Visibility = Visibility.Visible;
            buttonScoreboard.Visibility = Visibility.Visible;
            buttonQuit.Visibility = Visibility.Visible;
        }
        private void HideTheMainMenu()
        {
            buttonStartGame.Visibility = Visibility.Hidden;
            buttonScoreboard.Visibility = Visibility.Hidden;
            buttonQuit.Visibility = Visibility.Hidden;
        }
        //+Scoreboard - Show/Hide 
        private void ShowTheScoreboard()
        {
            foreach (Label item in scoreboardNo)
                item.Visibility = Visibility.Visible;
            foreach (Label item in scoreboardNames)
                item.Visibility = Visibility.Visible;
            foreach (Label item in scoreboardScore)
                item.Visibility = Visibility.Visible;
            foreach (Border item in scoreboardBorders)
                item.Visibility = Visibility.Visible;
            buttonScoreboardBack.Visibility = Visibility.Visible;
        }
        private void HideTheScoreboard()
        {
            foreach (Label item in scoreboardNo)
                item.Visibility = Visibility.Hidden;
            foreach (Label item in scoreboardNames)
                item.Visibility = Visibility.Hidden;
            foreach (Label item in scoreboardScore)
                item.Visibility = Visibility.Hidden;
            foreach (Border item in scoreboardBorders)
                item.Visibility = Visibility.Hidden;
            buttonScoreboardBack.Visibility = Visibility.Hidden;
        }

        //Actions Buttons 
        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            HideTheMainMenu();
            StartGame();
        }
        private void buttonScoreboard_Click(object sender, RoutedEventArgs e)
        {
            HideTheMainMenu();
            ShowTheScoreboard();
        }
        private void buttonQuit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to leave the game?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
        private void buttonScoreboardBack_Click(object sender, RoutedEventArgs e)
        {
            HideTheScoreboard();
            ShowTheMainMenu();
        }
        //Creating items 
        private void CreateTextBoxCol(ref Label[] label, int count, double x, double y, double tbWidth, double tbHeight)
        {
            label = new Label[count];
            for (int i = 0; i < count; i++)
            {
                label[i] = CreateLabel("", x, y + i * (tbHeight - 1), tbWidth, tbHeight);
                menuGrid.Children.Add(label[i]);

                Border border = CreateBorder(x, y + i * (tbHeight - 1), tbWidth, tbHeight);
                scoreboardBorders.Add(border);
                menuGrid.Children.Add(border);
            }
        }
        private Label CreateLabel(string text, double x, double y, double tbWidth, double tbHeight)
        {
            Label label = new Label();
            label.Margin = new Thickness((int)x, (int)y, 0.0, 0.0);
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Content = text;
            label.Width = (int)tbWidth;
            label.Height = (int)tbHeight;

            return label;
        }
        private Border CreateBorder(double x, double y, double bWidth, double bHeight)
        {
            Border border = new Border();
            border.Margin = new Thickness((int)x, (int)y, 0.0, 0.0);
            border.BorderThickness = new Thickness(1, 1, 1, 1);
            border.BorderBrush = Brushes.Black;
            border.HorizontalAlignment = HorizontalAlignment.Left;
            border.VerticalAlignment = VerticalAlignment.Top;
            border.Width = (int)bWidth;
            border.Height = (int)bHeight;

            return border;
        }
        private Button CreateButton(string text, RoutedEventHandler clickEvent, double x, double y, double bWidth, double bHeight)
        {
            Button button = new Button();
            button.Margin = new Thickness((int)x, (int)y, 0.0, 0.0);
            button.Content = text;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Width = bWidth;
            button.Height = bHeight;
            button.Click += clickEvent;

            return button;
        }
        

        //GAME 
        public void StartGame()
        {
            CompositionTarget.Rendering += MainLoopFunction;
            CreateWalls();
            GameControl.StartGame(ref gameGrid, ref platform, ref balls, ref bricks, ref bonuses);
        }

        public void FinishGame()
        {
            CompositionTarget.Rendering -= MainLoopFunction;
            walls.Clear();
            gameGrid.Children.Clear();
            ShowTheMainMenu();
        }

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
                (new Uri("./Graphics/background_wall.png", UriKind.Relative), gameGrid, 0, 0, 160, 600);
            TransformingImage rightWallImg = new TransformingImage
                (new Uri("./Graphics/background_wall.png", UriKind.Relative), gameGrid, 640, 0, 160, 600);
        }

        private void MainLoopFunction(Object Sender, EventArgs e)
        {
            GameControl.CheckGameState(ref gameGrid, ref platform, ref balls, ref bricks, ref bonuses);
            GameControl.RefreshStatistics(ref gameGrid);
            DoElementsActions();
            if (GameControl.IsTheGameOver())
                FinishGame();
        }

        private void RemoveElement<T>(List<T> listOfElements, T element) where T : UIElement
        {
            gameGrid.Children.Remove(element);
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
                    brick.Collisions(gameGrid, balls, bricks, destroyedBricks);
            }
            foreach (Brick brick in destroyedBricks)
            {
                if (Bonus.OrCreate())
                    bonuses.Add(new Bonus(gameGrid, Bonus.Random(), brick.Margin.Left, brick.Margin.Top));
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
            platform.Control(Mouse.GetPosition(this), IsLeftMouseButtonPressed(), balls);
            platform.Collisions(ref gameGrid, ref balls, ref bonuses);
        }
        private void DoElementsActions()
        {
            BallsActions();
            BricksActions();
            BonusesActions();
            PlatformActions();
        }

        public bool IsLeftMouseButtonPressed()
        {
            return (Mouse.LeftButton == MouseButtonState.Pressed ? true : false);
        }
    }
}
