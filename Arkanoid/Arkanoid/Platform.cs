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

        public double Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        //-------------------------------------------------------
        public Platform(Grid grid)
        {
            _platformLeft = new TransformingImage(new Uri("./Graphics/platform_left.png", UriKind.Relative), grid, 367, 552);
            _platformMiddle = new TransformingImage(new Uri("./Graphics/platform_middle.png", UriKind.Relative), grid, 399, 552, 2);
            _platformRight = new TransformingImage(new Uri("./Graphics/platform_right.png", UriKind.Relative), grid, 401, 552);

            _speed = 5.0;
        }
        //-------------------------------------------------------
        private void Move(double x, double y = 0.0)
        {
            _platformLeft.Move(x, y);
            _platformMiddle.Move(x, y);
            _platformRight.Move(x, y);
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

        public void Collisions(ref Ball ball)
        {
            double x = _platformLeft.Margin.Left;
            double y = _platformLeft.Margin.Top;
            double width = _platformRight.Margin.Left + _platformRight.Width - _platformLeft.Margin.Left;
            double height = _platformLeft.Height;
            Rect collisionArea = new Rect(x, y, width, height);
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

                ball.SetPosition(ball.Margin.Left, _platformMiddle.Margin.Top - _platformMiddle.Height);
            }
        }
    }
}
