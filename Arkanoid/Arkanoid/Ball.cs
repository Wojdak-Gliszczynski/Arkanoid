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
        private Rect lastCollisionArea; //Obszar kolizji z poprzedniego ruchu
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
            set
            {
                while (value < 0)
                {
                    value += 2 * Math.PI;
                }
                _angle = value % (2 * Math.PI);
            }
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
            lastCollisionArea = new Rect(Margin.Left, Margin.Top, Width, Height);

            double speedX = Math.Cos(Angle) * Speed;
            double speedY = Math.Sin(Angle) * Speed;

            this.Move(speedX, speedY);
        }
        //-------------------------------------------------------
        public void Bounce(Rect rect)
        {
            //Z której części prostokąta się odbija (przejście między ćwiartkami)
            if ((lastCollisionArea.Top >= rect.Bottom) || (lastCollisionArea.Bottom <= rect.Top))    //Odbija się od dolnej lub górnej części prostokąta
                Angle = Math.PI - (Angle - Math.PI);
            else if ((lastCollisionArea.Right <= rect.Left) || (lastCollisionArea.Left >= rect.Right))   //Odbija się od lewej lub prawej części prostokąta
                Angle = Math.PI * 0.5 - (Angle - Math.PI * 0.5);

            Move(); //Wymagany, żeby piłka nie znajdywała się cały czas wewnątrz prostokąta
        }
    }
}
