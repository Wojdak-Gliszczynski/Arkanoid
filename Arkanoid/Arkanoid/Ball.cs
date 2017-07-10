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
        private Rect _lastCollisionArea; //Obszar kolizji z poprzedniego ruchu
        private Rect _collisionArea;    //Obszar kolizji teraz
        private double _speed;
        private double _angle;   //W radianach
        private int _sizeDegree;
        Grid _gridAddress;  //Dla konstruktora kopiującego

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value >= 2 && value <= 6)
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
            Speed = 4;
            Angle = (1.0 / 4.0) * 2 * Math.PI;
            _sizeDegree = 3;
            AdjustSize();
            _gridAddress = grid;
        }

        public Ball(Ball ball) : base(new Uri("./Graphics/ball-0.png", UriKind.Relative), ball._gridAddress, ball.Margin.Left, ball.Margin.Top, ball.Width, ball.Height)
        {
            Speed = ball.Speed;
            Angle = ball.Angle;
            _sizeDegree = ball._sizeDegree;
            AdjustSize();
            _gridAddress = ball._gridAddress;

            //Zmiana toru lotu obu piłeczek
            ball.Angle -= Math.PI * 0.25;
            Angle += Math.PI * 0.25;
        }
        //-------------------------------------------------------
        public void Move()
        {
            _lastCollisionArea = new Rect(Margin.Left, Margin.Top, Width, Height);

            double speedX = Math.Cos(Angle) * Speed;
            double speedY = Math.Sin(Angle) * Speed;

            this.Move(speedX, speedY);

            _collisionArea = new Rect(Margin.Left, Margin.Top, Width, Height);
        }

        public void Bounce(Rect rect)
        {
            //Z której części prostokąta się odbija (przejście między ćwiartkami)
            if ((_lastCollisionArea.Top >= rect.Bottom) || (_lastCollisionArea.Bottom <= rect.Top))    //Odbija się od dolnej lub górnej części prostokąta
                Angle = Math.PI - (Angle - Math.PI);
            else if ((_lastCollisionArea.Right <= rect.Left) || (_lastCollisionArea.Left >= rect.Right))   //Odbija się od lewej lub prawej części prostokąta
                Angle = Math.PI * 0.5 - (Angle - Math.PI * 0.5);

            SetPosition(_lastCollisionArea.Left, _lastCollisionArea.Top);
            //Move(); //Wymagany, żeby piłka nie znajdywała się cały czas wewnątrz prostokąta
        }

        public bool HasCollisionWith(Rect rect)
        {
            return (_collisionArea.IntersectsWith(rect));
        }

        public bool IsDestroyed()
        {
            return (Margin.Top > 600 ? true : false);
        }

        private void AdjustSize()
        {
            if (_sizeDegree < 1)
                _sizeDegree = 1;
            else if (_sizeDegree > 5)
                _sizeDegree = 5;

            switch (_sizeDegree)
            {
                case 1: ChangeSize(0.25); break;
                case 2: ChangeSize(0.5); break;
                case 3: ChangeSize(1.0); break;
                case 4: ChangeSize(1.5); break;
                case 5: ChangeSize(2.0); break;
            }
        }

        private void ChangeSize(double scale)
        {
            double lastSize = Width;

            Width = 16 * scale;
            Height = 16 * scale;
            
            Margin = new Thickness(Margin.Left - (Width - lastSize), Margin.Top - (Height - lastSize), 0, 0);
        }

        public void SizeUp()
        {
            _sizeDegree++;
            AdjustSize();
        }

        public void SizeDown()
        {
            _sizeDegree--;
            AdjustSize();
        }
    }
}
