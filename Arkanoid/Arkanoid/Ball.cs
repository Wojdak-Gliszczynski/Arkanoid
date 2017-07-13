using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Arkanoid
{
    class Ball : TransformingImage
    {
        private Rect _prevCollisionArea;    //Collision area in the previous frame
        private Rect _collisionArea;    //Collision area now
        private double _speed;
        private double _angle;  //Radians
        private int _sizeDegree;
        private bool _glued;    //Is the ball glued to a platform?

        public double Speed
        {
            get { return _speed; }
            set
            {
                if (value < 2)
                    _speed = 2;
                else if (value > 6)
                    _speed = 6;
                else
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
        
        //CONSTRUCTORS 
        public Ball(Grid grid, Platform platfrom) 
            : base(new Uri("./Graphics/ball-0.png", UriKind.Relative), grid, platfrom.GetCenterX() - 8, 552 - 16)
        {
            SetInitialParameters(4, (3.0 / 4.0) * 2 * Math.PI, 3);
            _glued = true;
        }
        public Ball(Ball ball) 
            : base (
                  new Uri("./Graphics/ball-0.png", UriKind.Relative), 
                  VisualTreeHelper.GetParent(ball) as Grid, 
                  ball.Margin.Left, 
                  ball.Margin.Top, 
                  ball.Width, 
                  ball.Height
                  )
        {
            SetInitialParameters(ball.Speed, ball.Angle, ball._sizeDegree);
            _glued = ball._glued;
            
            //Zmiana toru lotu obu piłeczek
            ball.Angle -= Math.PI * 0.25;
            Angle += Math.PI * 0.25;
        }
        public Ball(Grid grid, double speed, double angle, double x, double y) 
            : base(new Uri("./Graphics/ball-0.png", UriKind.Relative), grid, x, y)
        {
            SetInitialParameters(speed, angle, 3);
            _glued = false;
        }
        //-------------------------------------------------------
        private void SetInitialParameters(double speed, double angle, int sizeDegree)
        {
            Speed = speed;
            Angle = angle;
            _sizeDegree = sizeDegree;
            AdjustSize();
        }
        
        //RESIZE FUNCTIONS 
        private void ChangeSize(double scale)
        {
            double lastSize = Width;

            Width = 16 * scale;
            Height = 16 * scale;

            Margin = new Thickness(Margin.Left - (Width - lastSize), Margin.Top - (Height - lastSize), 0, 0);
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
        //CONTROL FUNCTIONS 
        public void Move()
        {
            _prevCollisionArea = new Rect(Margin.Left, Margin.Top, Width, Height);
            if (!_glued)
            {
                double speedX = Math.Cos(Angle) * Speed;
                double speedY = Math.Sin(Angle) * Speed;

                this.Move(speedX, speedY);

                _collisionArea = new Rect(Margin.Left, Margin.Top, Width, Height);
            }
        }
        public void Bounce(Rect rect)
        {
            //From whitch part of the rectangle does it bounce? (Transition between quarters)
            //It bounces from the bottom or top of the rectangle
            if ((_prevCollisionArea.Top >= rect.Bottom) || (_prevCollisionArea.Bottom <= rect.Top))
                Angle = Math.PI - (Angle - Math.PI);
            //It bounces from the left or right part of the rectangle
            else if ((_prevCollisionArea.Right <= rect.Left) || (_prevCollisionArea.Left >= rect.Right))
                Angle = Math.PI * 0.5 - (Angle - Math.PI * 0.5);
            //Return to the previous position
            SetPosition(_prevCollisionArea.Left, _prevCollisionArea.Top);
        }
        public void Peel()
        {
            _glued = false;
        }

        public bool HasCollisionWith(Rect rect)
        {
            return (_collisionArea.IntersectsWith(rect));
        }
        public bool IsDestroyed()
        {
            return (Margin.Top > 600 ? true : false);
        }
        public bool IsGlued()
        {
            return _glued;
        }
    }
}
