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
    public partial class TransformingImage : Image
    {
        public TransformingImage(Uri uri, Grid grid, int x = 0, int y = 0, double width = 32, double height = 32)
        {
            BitmapImage bmp = new BitmapImage(uri);
            this.Source = bmp;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = width;
            this.Height = height;
            this.Margin = new Thickness(x, y, 0.0, 0.0);
            grid.Children.Add(this);
        }

        public void Move(double right, double top)
        {
            Thickness margin = new Thickness();
            margin = this.Margin;
            margin.Left += right;
            margin.Top += top;
            this.Margin = margin;
        }
    }

    public partial class Ball : TransformingImage
    {
        private double _speed;
        private double _angle;   //W radianach

        public Ball(Grid grid) : base(new Uri("./Graphics/ball-0.png", UriKind.Relative), grid, 384, 456)
        {
            _speed = 5;
            _angle = (3.0 / 4.0) * Math.PI;
        }
    }

    public partial class Platform
    {
        private TransformingImage _platformLeft, _platformMiddle, _platformRight;
        private static double _lastMouseX;  //Zmienna wymagana gdy kursor wyjdzie poza ekran
        private double _speed;

        private void Move(double right, double top = 0.0)
        {
            _platformLeft.Move(right, top);
            _platformMiddle.Move(right, top);
            _platformRight.Move(right, top);
        }

        public Platform(Grid grid)
        {
            _platformLeft = new TransformingImage(new Uri("./Graphics/platform_left.png", UriKind.Relative), grid, 367, 552);
            _platformMiddle = new TransformingImage(new Uri("./Graphics/platform_middle.png", UriKind.Relative), grid, 399, 552, 2);
            _platformRight = new TransformingImage(new Uri("./Graphics/platform_right.png", UriKind.Relative), grid, 401, 552);

            _speed = 5.0;
        }

        public void Control(Point mousePosition)
        {
            double platformX = _platformMiddle.Margin.Left + (_platformMiddle.Width / 2.0);

            if (mousePosition.X > 0 && mousePosition.X < 800)
                _lastMouseX = mousePosition.X;

            if (_lastMouseX < platformX - _speed / 2.0)
                Move(-_speed);
            else if (_lastMouseX > platformX + _speed / 2.0)
                Move(_speed);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Platform platform;
        private Ball ball;

        public MainWindow()
        {
            InitializeComponent();
            //Timer
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
            //Platform
            platform = new Platform(grid1);
            //Ball
            ball = new Ball(grid1);
            //grid1.Children.Add(ball);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            platform.Control(Mouse.GetPosition(this));
        }
    }
}
