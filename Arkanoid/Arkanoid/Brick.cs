using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Arkanoid
{
    class Brick : TransformingImage
    {
        private ushort _typeID;
        private ushort _colorID;
        
        public enum BrickType : ushort { Normal = 1, Reinforced, Indestructible, Ball, TNT };
        public enum BrickColor : ushort { Black = 1, Red, Orange, Yellow, Green, Cyan, Blue, Purple, Magenta, White };

        public BrickType Type
        {
            get { return (BrickType)_typeID; }
        }

        public BrickColor Color
        {
            get { return (BrickColor)_colorID; }
        }

        public Brick(ref Grid grid, ushort typeID, ushort colorID, int xOnGrid, int yOnGrid) : base(GetPath(typeID, colorID), grid, 160 + xOnGrid * 32 - 32, yOnGrid * 16, 32, 16)
        {
            _typeID = typeID;
            _colorID = colorID;
        }

        static private Uri GetPath(ushort typeID, ushort colorID)
        {
            string path = "./Graphics/";
            switch(typeID)
            {
                case (int)BrickType.Normal: path += "brick-"; break;
                case (int)BrickType.Reinforced: path += "reinforced_brick-"; break;
                case (int)BrickType.Indestructible: path += "indestructible_brick-"; break;
                case (int)BrickType.Ball: path += "ball_brick-"; break;
                case (int)BrickType.TNT: return new Uri(path + "tnt_brick.png", UriKind.Relative);
            }
            path += colorID + ".png";

            return new Uri(path, UriKind.Relative);
        }

        public bool Collisions(ref Grid grid, ref List <Ball> balls, ref List <Brick> bricks, ref List <Explosion> explosions, ref List <Brick> destroyedBricks) //TRUE - cegiełka istnieje; FALSE - cegiełka została zniszczona
        {
            Rect collisionsArea = new Rect(Margin.Left, Margin.Top, Width, Height);

            foreach (Ball ball in balls)
            {
                Rect ballArea = new Rect(ball.Margin.Left, ball.Margin.Top, ball.Width, ball.Height);

                if (collisionsArea.IntersectsWith(ballArea))
                {
                    double ballAngle = ball.Angle;
                    ball.Bounce(collisionsArea);

                    switch (_typeID)
                    {
                        case (int)BrickType.Normal:
                            destroyedBricks.Add(this);
                            GameControl.AddPoints(25);
                            return false;
                        case (int)BrickType.Reinforced:
                            GameControl.AddPoints(10);
                            _typeID = (ushort)BrickType.Normal;
                            Source = new BitmapImage(GetPath(_typeID, _colorID));
                            break;
                        case (int)BrickType.Ball:
                            destroyedBricks.Add(this);
                            GameControl.AddPoints(15);
                            balls.Add(new Ball(grid, ball.Speed, ballAngle, Margin.Left + Width / 2 - 8, Margin.Top + Height / 2 - 8));
                            return false;
                        case (int)BrickType.TNT:
                            destroyedBricks.Add(this);
                            GameControl.AddPoints(5);
                            explosions.Add(new Explosion(ref grid, ref balls, ref bricks, this, ref destroyedBricks, ref explosions));
                            return false;
                    }
                }
            }
            return true;
        }

        public void DestroyInExplosion(ref Grid grid, ref List<Ball> balls, ref List<Brick> bricks, ref List<Explosion> explosions, ref List<Brick> destroyedBricks)
        {
            destroyedBricks.Add(this);
            switch (_typeID)
            {
                case (int)BrickType.Normal:
                    GameControl.AddPoints(25);
                    destroyedBricks.Add(this);
                    break;
                case (int)BrickType.Reinforced:
                    GameControl.AddPoints(35);
                    destroyedBricks.Add(this);
                    break;
                case (int)BrickType.Ball:
                    GameControl.AddPoints(15);
                    balls.Add(new Ball(grid, 3, Convert.ToDouble(new Random().Next(0, (int)(2 * Math.PI * 100))) / 100, Margin.Left + Width / 2 - 8, Margin.Top + Height / 2 - 8));
                    destroyedBricks.Add(this);
                    break;
                case (int)BrickType.TNT:
                    GameControl.AddPoints(5);
                    explosions.Add(new Explosion(ref grid, ref balls, ref bricks, this, ref destroyedBricks, ref explosions));
                    destroyedBricks.Add(this);
                    break;
            }
        }
    }
}
