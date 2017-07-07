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
        private int _id;
        static int _speed = 2;

        public Bonus(Grid grid, int ID, double x, double y) : base(new Uri("./Graphics/bonus-" + ID + ".png", UriKind.Relative), grid, x, y)
        {
            _id = ID;
        }

        static public bool OrCreate()
        {
            Random rand = new Random();
            if (rand.Next(0, 4) == 0)
                return true;
            return false;
        }

        static public int RandomID()
        {
            Random rand = new Random();
            return rand.Next(0, 13);
        }

        public void Move()
        {
            Move(0, _speed);
        }

        public void UseBonus(ref Grid grid, ref Platform platform, ref List<Ball> balls)
        {
            Random rand = new Random();
            if (_id == 0)
                _id = rand.Next(1, 13);

            switch (_id)
            {
                case 1:
                    for (int i = 0; i < Math.Ceiling(Convert.ToDouble(balls.Count) / 2); i++)    //Dwa razy mniejszy zakres, gdyż będą powstawały nowe piłeczki
                        balls.Add(new Ball(balls[i]));
                    break;
                case 2:
                    foreach (Ball ball in balls)
                        ball.SizeUp();
                    break;
                case 3:
                    foreach (Ball ball in balls)
                        ball.SizeDown();
                    break;
                case 4:
                    foreach (Ball ball in balls)
                        ball.Speed--;
                    break;
                case 5:
                    foreach (Ball ball in balls)
                        ball.Speed++;
                    break;
                case 6:
                    platform.SizeUp();
                    break;
                case 7:
                    platform.SizeDown();
                    break;
                case 8:
                    platform.Speed++;
                    break;
                case 9:
                    platform.Speed--;
                    break;
                case 10:
                    GameControl.AddLife();
                    break;
                case 11:
                    GameControl.AddPoints(1000);
                    break;
                case 12:
                    foreach (Ball ball in balls)
                        grid.Children.Remove(ball);
                    balls.Clear();
                    break;
            }

            grid.Children.Remove(this);
        }
    }
}
