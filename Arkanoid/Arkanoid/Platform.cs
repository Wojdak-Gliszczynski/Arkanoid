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
    class Platform
    {
        private TransformingImage _platformLeft, _platformMiddle, _platformRight;
        private static double _lastMouseX;  //The variable required when the cursor leaves the screen
        private double _speed;
        private int _sizeDegree;

        public double Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        //-------------------------------------------------------
        public Platform(Grid grid)
        {
            _platformLeft = new TransformingImage(new Uri("./Graphics/platform_left.png", UriKind.Relative), grid, 268, 552);
            _platformMiddle = new TransformingImage(new Uri("./Graphics/platform_middle.png", UriKind.Relative), grid, 300, 552, 0);
            _platformRight = new TransformingImage(new Uri("./Graphics/platform_right.png", UriKind.Relative), grid, 301, 552);

            _speed = 3;
            _sizeDegree = 3;
            RefreshSize(grid.Width / 2);
        }
        //-------------------------------------------------------
        //CONTROL FUNCTIONS 
        private void Move(double x, double y = 0.0)
        {
            _platformLeft.Move(x, y);
            _platformMiddle.Move(x, y);
            _platformRight.Move(x, y);
        }
        private void MoveGluedBalls(List<Ball> balls, double x, double y = 0.0)
        {
            foreach (Ball ball in balls)
                if (ball.IsGlued())
                    ball.Move(x, y);
        }
        private void MoveWithBalls(List<Ball> balls, double x, double y = 0.0)
        {
            Move(x, y);
            MoveGluedBalls(balls, x, y);
        }
        private void SetPosition(double x, double y = 552.0)
        {
            _platformLeft.SetPosition(x, y);
            _platformMiddle.SetPosition(x + _platformLeft.Width - 1, y);
            _platformRight.SetPosition(x + _platformLeft.Width + _platformMiddle.Width - 1, y);
        }
        public void Control(Point mousePosition, bool leftMouseButtonPressed, List<Ball> balls)
        {
            double platformX = _platformMiddle.Margin.Left + (_platformMiddle.Width / 2.0);

            if (mousePosition.X > 0 && mousePosition.X < 800)
                _lastMouseX = mousePosition.X;

            if (_lastMouseX <= platformX - _speed)
                MoveWithBalls(balls, -_speed);
            else if (_lastMouseX >= platformX + _speed)
                MoveWithBalls(balls, _speed);
            else //Move less pixels than the platform speed
                MoveWithBalls(balls, _lastMouseX - platformX);

            if (_platformLeft.Margin.Left < 160)
                MoveWithBalls(balls, 160 - (_platformLeft.Margin.Left));
            else if (_platformRight.Margin.Left + _platformRight.Width > 640)
                MoveWithBalls(balls, 640 - (_platformRight.Margin.Left + _platformRight.Width));

            if (leftMouseButtonPressed)
                PeelGluedBalls(balls);
        }
        //COLLISIONS 
        private Rect GetCollisionArea()
        {
            double x = _platformLeft.Margin.Left;
            double y = _platformLeft.Margin.Top;
            double width = _platformRight.Margin.Left + _platformRight.Width - _platformLeft.Margin.Left;
            double height = _platformLeft.Height;
            return new Rect(x, y, width, height);
        }
        private void CollisionsWithBalls(ref List<Ball> balls)
        {
            Rect collisionArea = GetCollisionArea();

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

                    if (!ball.IsGlued())
                        GameControl.PlaySound(GameControl.Sound.BouncingBall);
                }
            }
        }
        private void CollisionsWithBonuses(ref Grid grid, ref List<Ball> balls, ref List<Bonus> bonuses)
        {
            Rect collisionArea = GetCollisionArea();

            for (int i = bonuses.Count - 1; i >= 0; i--)
            {
                Rect bonusArea = new Rect(bonuses[i].Margin.Left, bonuses[i].Margin.Top, bonuses[i].Width, bonuses[i].Height);
                if (collisionArea.IntersectsWith(bonusArea))
                {
                    var thisPlatform = this;
                    bonuses[i].UseBonus(ref grid, ref thisPlatform, ref balls);
                    if (bonuses.Count > 0)  //W przypadku podniesienia bonusu przez który znikają wszystkie bonusy (np. utrata życia)
                        bonuses.RemoveAt(i);
                }
            }
        }
        public void Collisions(ref Grid grid, ref List <Ball> balls, ref List<Bonus> bonuses)
        {
            CollisionsWithBalls(ref balls);
            CollisionsWithBonuses(ref grid, ref balls, ref bonuses);
        }
        //RESIZE FUNCTIONS 
        public void RefreshSize(double xCenter)
        {
            ScaleTransform st = new ScaleTransform(SizeToPixels(_sizeDegree) + 2, 1, 0, 0);
            _platformMiddle.Width = SizeToPixels(_sizeDegree);
            _platformMiddle.RenderTransform = st;
            _platformMiddle.HorizontalAlignment = HorizontalAlignment.Left;

            SetPosition(xCenter - _platformMiddle.Width / 2.0 - _platformLeft.Width);
        }
        public void SizeUp()
        {
            _sizeDegree++;
            if (_sizeDegree > 5)
                _sizeDegree = 5;

            RefreshSize(_platformMiddle.Margin.Left + _platformMiddle.Width / 2);
        }
        public void SizeDown()
        {
            _sizeDegree--;
            if (_sizeDegree < 1)
                _sizeDegree = 1;

            RefreshSize(_platformMiddle.Margin.Left + _platformMiddle.Width / 2);
        }

        public void PeelGluedBalls(List<Ball> balls)
        {
            foreach (Ball ball in balls)
                if (ball.IsGlued())
                    ball.Peel();
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

        public double GetCenterX()
        {
            return _platformMiddle.Margin.Left + _platformMiddle.Width / 2.0;
        }
        
        private int SizeToPixels(int sizeDegree)
        {
            return (sizeDegree - 1) * 25;
        }
    }
}
