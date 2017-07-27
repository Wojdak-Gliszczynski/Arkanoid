using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Arkanoid
{
    class TransformingImage : Image
    {
        public TransformingImage(Uri uri, Grid grid, double x = 0, double y = 0, double width = 16, double height = 16)
        {
            this.Source = new BitmapImage(uri);
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = width;
            this.Height = height;
            SetPosition(x, y);
            grid.Children.Add(this);
        }
        //-------------------------------------------------------
        public void Move(double x, double y)
        {
            this.Margin = new Thickness(Margin.Left + x, Margin.Top + y, 0.0, 0.0);
        }

        public void SetPosition(double x, double y)
        {
            this.Margin = new Thickness(x, y, 0.0, 0.0);
        }
    }
}
