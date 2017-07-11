using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arkanoid
{
    class Bonus : TransformingImage
    {
        public enum BonusName : int
        {
            Random = 0,
            DoubleBall,
            BallSizeUp,
            BallSizeDown,
            BallSpeedDown,
            BallSpeedUp,
            PlatformSizeUp,
            PlatformSizeDown,
            PlatformSpeedUp,
            PlatformSpeedDown,
            ExtraLife,
            ExtraPoints,
            Skull
        }

        private BonusName _id;

        private static Random _rand = new Random(DateTime.Now.Millisecond);
        private static int _speed = 2;
        private static int _maxBonusID = 12;
        //-------------------------------------------------------
        public Bonus(Grid grid, BonusName ID, double x, double y) 
            : base(new Uri("./Graphics/bonus-" + (int)ID + ".png", UriKind.Relative), grid, x, y)
        {
            _id = ID;
        }
        //-------------------------------------------------------
        static public bool OrCreate()
        {
            return (_rand.Next(0, 4) == 0 ? true : false);
        }

        static public BonusName Random()
        {
            return (BonusName)_rand.Next(0, _maxBonusID + 1);
        }

        public void Move()
        {
            Move(0, _speed);
        }

        public void UseBonus(ref Grid grid, ref Platform platform, ref List<Ball> balls)
        {
            if (_id == BonusName.Random)
                _id = (BonusName)_rand.Next(1, _maxBonusID + 1);

            switch (_id)
            {
                case BonusName.DoubleBall:
                    int ballsCount = balls.Count;
                    for (int i = 0; i < ballsCount; i++)
                        balls.Add(new Ball(balls[i]));
                    break;
                case BonusName.BallSizeUp:
                    foreach (Ball ball in balls)
                        ball.SizeUp();
                    break;
                case BonusName.BallSizeDown:
                    foreach (Ball ball in balls)
                        ball.SizeDown();
                    break;
                case BonusName.BallSpeedDown:
                    foreach (Ball ball in balls)
                        ball.Speed--;
                    break;
                case BonusName.BallSpeedUp:
                    foreach (Ball ball in balls)
                        ball.Speed++;
                    break;
                case BonusName.PlatformSizeUp:
                    platform.SizeUp();
                    break;
                case BonusName.PlatformSizeDown:
                    platform.SizeDown();
                    break;
                case BonusName.PlatformSpeedUp:
                    platform.Speed++;
                    break;
                case BonusName.PlatformSpeedDown:
                    platform.Speed--;
                    break;
                case BonusName.ExtraLife:
                    GameControl.AddLife();
                    break;
                case BonusName.ExtraPoints:
                    GameControl.AddPoints(1000);
                    break;
                case BonusName.Skull:
                    foreach (Ball ball in balls)
                        grid.Children.Remove(ball);
                    balls.Clear();
                    break;
            }

            grid.Children.Remove(this);
        }
    }
}
