using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arkanoid
{
    class Platform
    {
        private TransformingImage _platformLeft, _platformMiddle, _platformRight;
        private static double _lastMouseX;  //Zmienna wymagana gdy kursor wyjdzie poza ekran
        private double _speed;
        private int _sizeDegree;

        public double Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        //-------------------------------------------------------
        public Platform(ref Grid grid)
        {
            _sizeDegree = 3;

            _platformLeft = new TransformingImage(new Uri("./Graphics/platform_left.png", UriKind.Relative), grid, 268, 552);
            _platformMiddle = new TransformingImage(new Uri("./Graphics/platform_middle.png", UriKind.Relative), grid, 300, 552, 0);
            _platformRight = new TransformingImage(new Uri("./Graphics/platform_right.png", UriKind.Relative), grid, 301, 552);

            RefreshPlatformGraphics(grid.Width / 2);
            RefreshSize();

            _speed = 3;
        }
        //-------------------------------------------------------
        private void Move(double x, double y = 0.0)
        {
            _platformLeft.Move(x, y);
            _platformMiddle.Move(x, y);
            _platformRight.Move(x, y);
        }

        private void SetPosition(double x, double y = 552.0)
        {
            _platformLeft.SetPosition(x, y);
            _platformMiddle.SetPosition(x + _platformLeft.Width + _platformMiddle.Width, y);
            _platformRight.SetPosition(x + _platformLeft.Width + _platformMiddle.Width, y);
        }

        public void Control(Point mousePosition)
        {
            double platformX = _platformMiddle.Margin.Left - (_platformMiddle.Width / 2.0);

            if (mousePosition.X > 0 && mousePosition.X < 800)
                _lastMouseX = mousePosition.X;

            if (_lastMouseX < platformX - _speed / 2.0)
            {
                Move(-_speed);
                if (_platformLeft.Margin.Left < 160)
                    SetPosition(160);
            }
            else if (_lastMouseX > platformX + _speed / 2.0)
            { 
                Move(_speed);
                if (_platformRight.Margin.Left + _platformRight.Width > 640)
                    SetPosition(640 - _platformRight.Width - _platformMiddle.Width - _platformLeft.Width);
            }
        }

        public void Collisions(ref List <Ball> balls)
        {
            double x = _platformLeft.Margin.Left;
            double y = _platformLeft.Margin.Top;
            double width = _platformRight.Margin.Left + _platformRight.Width - _platformLeft.Margin.Left;
            double height = _platformLeft.Height;
            Rect collisionArea = new Rect(x, y, width, height);

            foreach (Ball ball in balls)
            {
                Rect ballArea = new Rect(ball.Margin.Left, ball.Margin.Top, ball.Width, ball.Height);

                if (collisionArea.IntersectsWith(ballArea))
                {
                    double ballRelativeMiddleX = ballArea.Left + (ballArea.Width / 2.0) - collisionArea.Left;
                    double platformMiddleX = collisionArea.Width / 2.0;

                    double percent = Convert.ToDouble((ballRelativeMiddleX - platformMiddleX)) / (collisionArea.Width / 2.0);
                    if (percent > 0.8)
                        percent = 0.8;
                    else if (percent < -0.8)
                        percent = -0.8;

                    ball.Angle = ((3.0 / 2.0) * Math.PI) + percent * 0.5 * Math.PI;

                    ball.SetPosition(ball.Margin.Left, _platformMiddle.Margin.Top - ball.Height);
                }
            }
        }

        public int SizeToPixels(int sizeDegree)
        {
            return (sizeDegree - 1) * 25;
        }

        public void RefreshPlatformGraphics(double platformCenter)
        {
            double middleX = Convert.ToInt32(platformCenter - _platformMiddle.Width / 2.0);
            double leftX = Convert.ToInt32(middleX - _platformMiddle.Width - _platformLeft.Width);
            double rightX = Convert.ToInt32(middleX - _platformMiddle.Width + _platformMiddle.Width);

            _platformLeft.Margin = new Thickness(leftX, 552, 0, 0);
            _platformMiddle.Margin = new Thickness(middleX, 552, 0, 0);
            _platformRight.Margin = new Thickness(rightX, 552, 0, 0);
        }

        public void RefreshSize()
        {
            System.Windows.Media.ScaleTransform st = new System.Windows.Media.ScaleTransform(SizeToPixels(_sizeDegree) + 2, 1, 1, 1);
            _platformMiddle.Width = SizeToPixels(_sizeDegree);
            _platformMiddle.RenderTransform = st;

            RefreshPlatformGraphics(_platformMiddle.Margin.Left + _platformMiddle.Width / 2.0);
        }

        public void SizeUp()
        {
            _sizeDegree++;
            if (_sizeDegree > 5)
                _sizeDegree = 5;

            RefreshSize();
        }

        public void SizeDown()
        {
            _sizeDegree--;
            if (_sizeDegree < 1)
                _sizeDegree = 1;

            RefreshSize();
        }

        public void RemoveFromGrid(ref Grid grid)
        {
            if (grid.Children.Contains(_platformLeft))
                grid.Children.Remove(_platformLeft);

            if (grid.Children.Contains(_platformMiddle))
                grid.Children.Remove(_platformMiddle);

            if (grid.Children.Contains(_platformRight))
                grid.Children.Remove(_platformRight);
        }
    }
}
