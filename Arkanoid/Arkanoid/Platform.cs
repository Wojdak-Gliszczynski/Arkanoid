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
}
