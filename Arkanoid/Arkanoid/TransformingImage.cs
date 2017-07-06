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
        public TransformingImage(Uri uri, Grid grid, int x = 0, int y = 0, double width = 32, double height = 32)
        {
            BitmapImage bmp = new BitmapImage(uri);
            this.Source = bmp;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = width;
            this.Height = height;
            this.Margin = new Thickness(x, y, 0.0, 0.0);
            grid.Children.Add(this);
        }

        public void Move(double x, double y)
        {
            Thickness margin = new Thickness();
            margin = this.Margin;
            margin.Left += x;
            margin.Top += y;
            this.Margin = margin;
        }

        public void SetPosition(double x, double y)
        {
            Thickness margin = new Thickness();
            margin = this.Margin;
            margin.Left = x;
            margin.Top = y;
            this.Margin = margin;
        }
    }
}
