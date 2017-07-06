using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arkanoid
{
    class Ball : TransformingImage
    {
        private double _speed;
        private double _angle;   //W radianach

        public Ball(Grid grid) : base(new Uri("./Graphics/ball-0.png", UriKind.Relative), grid, 384, 456)
        {
            _speed = 5;
            _angle = (3.0 / 4.0) * Math.PI;
        }
    }
}
