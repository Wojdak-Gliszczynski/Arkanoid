using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arkanoid
{
    class Brick : TransformingImage
    {
        public Brick(ref Grid grid, ushort colorID, int xOnGrid, int yOnGrid) : base(new Uri("./Graphics/brick-" + colorID + ".png", UriKind.Relative), grid, 160 + xOnGrid * 32 - 32, yOnGrid * 16, 32, 16)
        {
            ;
        }

        public bool Collisions(ref List <Ball> balls) //TRUE - cegiełka istnieje; FALSE - cegiełka została zniszczona
        {
            Rect collisionsArea = new Rect(Margin.Left, Margin.Top, Width, Height);

            foreach(Ball ball in balls)
            {
                Rect ballArea = new Rect(ball.Margin.Left, ball.Margin.Top, ball.Width, ball.Height);

                if (collisionsArea.IntersectsWith(ballArea))
                {
                    ball.Bounce(collisionsArea);
                    GameControl.AddScore(10);
                    return false;
                }
            }
            return true;
        }
    }
}
