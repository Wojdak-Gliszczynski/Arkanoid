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
        public Brick(Grid grid, ushort colorID, int xOnGrid, int yOnGrid) : base(new Uri("./Graphics/brick-" + colorID + ".png", UriKind.Relative), grid, 160 + xOnGrid * 32, yOnGrid * 16, 32, 16)
        {
            ;
        }
    }
}
