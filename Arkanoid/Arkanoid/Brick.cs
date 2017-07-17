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
        public enum BrickType : int { Normal = 1, Reinforced, Indestructible, Ball, TNT };
        public enum BrickColor : int { Black = 1, Red, Orange, Yellow, Green, Cyan, Blue, Purple, Magenta, White };

        private BrickType _typeID;
        private BrickColor _colorID;

        public BrickType Type
        {
            get { return _typeID; }
        }

        public BrickColor Color
        {
            get { return _colorID; }
        }
        //-------------------------------------------------------
        public Brick(ref Grid grid, BrickType typeID, BrickColor colorID, int xOnGrid, int yOnGrid) 
            : base(GetPath(typeID, colorID), grid, 160 + (xOnGrid - 1) * 32, (yOnGrid - 1) * 16, 32, 16)
        {
            _typeID = typeID;
            _colorID = colorID;
        }
        //-------------------------------------------------------
        static private Uri GetPath(BrickType typeID, BrickColor colorID)
        {
            string path = "./Graphics/";
            switch(typeID)
            {
                case BrickType.Normal: path += "brick-"; break;
                case BrickType.Reinforced: path += "reinforced_brick-"; break;
                case BrickType.Indestructible: path += "indestructible_brick-"; break;
                case BrickType.Ball: path += "ball_brick-"; break;
                case BrickType.TNT: return new Uri(path + "tnt_brick.png", UriKind.Relative);
            }
            path += (int)colorID + ".png";

            return new Uri(path, UriKind.Relative);
        }
        
        public void Collisions(Grid grid, List<Ball> balls, List<Brick> bricks, List<Brick> destroyedBricks)
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
                        case BrickType.Normal:
                            GameControl.AddPoints(25);
                            destroyedBricks.Add(this);
                            return;
                        case BrickType.Reinforced:
                            GameControl.AddPoints(10);
                            _typeID = BrickType.Normal;
                            Source = new BitmapImage(GetPath(_typeID, _colorID));
                            break;
                        case BrickType.Ball:
                            GameControl.AddPoints(15);
                            destroyedBricks.Add(this);
                            balls.Add(new Ball(grid, ball.Speed, ballAngle, Margin.Left + Width / 2 - 8, Margin.Top + Height / 2 - 8));
                            return;
                        case BrickType.TNT:
                            GameControl.AddPoints(5);
                            destroyedBricks.Add(this);
                            Explosion explosion = new Explosion(grid, balls, bricks, this, destroyedBricks);
                            return;
                    }
                }
            }
        }

        public void DestroyInExplosion(Grid grid, List<Ball> balls, List<Brick> bricks, List<Brick> destroyedBricks)
        {
            switch (_typeID)
            {
                case BrickType.Normal:
                    GameControl.AddPoints(25);
                    destroyedBricks.Add(this);
                    break;
                case BrickType.Reinforced:
                    GameControl.AddPoints(35);
                    destroyedBricks.Add(this);
                    break;
                case BrickType.Ball:
                    GameControl.AddPoints(15);
                    destroyedBricks.Add(this);

                    double newBallAngle = new Random().Next(0, (int)(2 * Math.PI * 100)) / 100;
                    double newBallX = Margin.Left + Width / 2 - 8;
                    double newBallY = Margin.Top + Height / 2 - 8;
                    balls.Add(new Ball(grid, 3, newBallAngle, newBallX, newBallY));

                    break;
                case BrickType.TNT:
                    GameControl.AddPoints(5);
                    destroyedBricks.Add(this);
                    Explosion explosion = new Explosion(grid, balls, bricks, this, destroyedBricks);
                    break;
            }
        }
    }
}
