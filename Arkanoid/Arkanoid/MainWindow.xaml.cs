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
        private Ball ball;
        private Brick[] brick;

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
            ball = new Ball(grid1);
            brick = new Brick[5];
            brick[0] = new Brick(grid1, 0, 1, 1);
            brick[1] = new Brick(grid1, 1, 1, 2);
            brick[2] = new Brick(grid1, 2, 2, 1);
            brick[3] = new Brick(grid1, 3, 2, 2);
            brick[4] = new Brick(grid1, 4, 4, 1);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ball.Move();
            platform.Control(Mouse.GetPosition(this));

            platform.Collisions(ref ball);
        }
    }
}
