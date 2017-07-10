using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Arkanoid
{
    class Explosion : TransformingImage
    {
        Grid _gridAddress;
        DispatcherTimer _timer;

        public Explosion(ref Grid grid, ref List<Ball> balls, ref List<Brick> bricks, Brick brokenBrick, ref List<Brick> destroyedBricks, ref List<Explosion> explosions) : base(new Uri("./Graphics/explosion.png", UriKind.Relative), grid, brokenBrick.Margin.Left + brokenBrick.Width / 2 - 48, brokenBrick.Margin.Top + brokenBrick.Height / 2 - 24, 96, 48)
        {
            Rect collisionsArea = new Rect(Margin.Left, Margin.Top, Width, Height);
            for (int i = bricks.Count - 1; i >= 0; i--)
            {
                if (bricks[i].Type != Brick.BrickType.Indestructible && bricks[i] != brokenBrick && !destroyedBricks.Contains(bricks[i]))
                {
                    Rect brickCollisionsArea = new Rect(bricks[i].Margin.Left, bricks[i].Margin.Top, bricks[i].Width, bricks[i].Height);

                    if (collisionsArea.IntersectsWith(brickCollisionsArea))
                        bricks[i].DestroyInExplosion(ref grid, ref balls, ref bricks, ref explosions, ref destroyedBricks);
                }
            }

            _gridAddress = grid;

            //Timer istnienia efektu
            _timer = new DispatcherTimer();
            _timer.Tick += RemoveEffect;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        public void RemoveEffect(Object sender, EventArgs e)
        {
            _gridAddress.Children.Remove(this);
            _timer.Stop();
        }
    }
}
