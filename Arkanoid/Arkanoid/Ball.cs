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

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value >= 0)
                    _speed = value;
            }
        }
        public double Angle
        {
            get { return _angle; }
            set { _angle = value % (2 * Math.PI); }
        }
        //-------------------------------------------------------
        public Ball(Grid grid) : base(new Uri("./Graphics/ball-0.png", UriKind.Relative), grid, 384, 456)
        {
            Speed = 5;
            Angle = (1.0 / 4.0) * 2 * Math.PI;
        }
        //-------------------------------------------------------
        public void Move()
        {
            double speedX = Math.Cos(Angle) * Speed;
            double speedY = Math.Sin(Angle) * Speed;

            this.Move(speedX, speedY);
        }
    }
}
