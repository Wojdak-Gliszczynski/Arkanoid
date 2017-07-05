using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Arkanoid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double platformSpeed = 5.0;
        private double lastMouseX;  //Zmienna wymagana gdy kursor wyjdzie poza ekran

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            PlatformControl();
        }

        private void move(ref Image img, double right, double top)
        {
            Thickness margin = new Thickness();
            margin = img.Margin;
            margin.Left += right;
            margin.Top += top;
            img.Margin = margin;
        }

        private void PlatformControl()
        {
            double platformX = platformMiddle.Margin.Left + (platformMiddle.Width / 2.0);
            
            if (Mouse.GetPosition(this).X > 0 && Mouse.GetPosition(this).X < 800)
                lastMouseX = Mouse.GetPosition(this).X;

            if (lastMouseX < platformX - platformSpeed / 2.0)
            {
                move(ref platformLeft, -platformSpeed, 0.0);
                move(ref platformMiddle, -platformSpeed, 0.0);
                move(ref platformRight, -platformSpeed, 0.0);
            }
            else if (lastMouseX > platformX + platformSpeed / 2.0)
            {
                move(ref platformLeft, platformSpeed, 0.0);
                move(ref platformMiddle, platformSpeed, 0.0);
                move(ref platformRight, platformSpeed, 0.0);
            }
        }
    }
}
