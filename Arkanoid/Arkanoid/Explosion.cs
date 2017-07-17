using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Arkanoid
{
    class Explosion : TransformingImage
    {
        private DispatcherTimer _timer;
        //-------------------------------------------------------
        public Explosion(Grid grid, List<Ball> balls, List<Brick> bricks, Brick brokenBrick, List<Brick> destroyedBricks) 
            : base (
                  new Uri("./Graphics/explosion.png", UriKind.Relative), 
                  grid, 
                  brokenBrick.Margin.Left + brokenBrick.Width / 2 - 48, 
                  brokenBrick.Margin.Top + brokenBrick.Height / 2 - 24, 
                  96, 48
                  )
        {
            Rect collisionsArea = new Rect(Margin.Left + 1, Margin.Top + 1, Width - 2, Height - 2);
            foreach (Brick brick in bricks)
            {
                if (brick.Type != Brick.BrickType.Indestructible && brick != brokenBrick && !destroyedBricks.Contains(brick))
                {
                    Rect brickCollisionsArea = new Rect(brick.Margin.Left, brick.Margin.Top, brick.Width, brick.Height);
                    if (collisionsArea.IntersectsWith(brickCollisionsArea))
                        brick.DestroyInExplosion(grid, balls, bricks, destroyedBricks);
                }
            }

            //Effect's timer
            _timer = new DispatcherTimer();
            _timer.Tick += RemoveEffect;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }
        //-------------------------------------------------------
        public void RemoveEffect(Object sender, EventArgs e)
        {
            (VisualTreeHelper.GetParent(this) as Grid).Children.Remove(this);
            _timer.Stop();
        }
    }
}
