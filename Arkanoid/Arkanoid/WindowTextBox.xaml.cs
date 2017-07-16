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
using System.Windows.Shapes;

namespace Arkanoid
{
    /// <summary>
    /// Interaction logic for WindowTextBox.xaml
    /// </summary>
    public partial class WindowTextBox : Window
    {
        public WindowTextBox(string description, string title = "")
        {
            InitializeComponent();
            Description.Text = description;
            Title = title;
            ShowDialog();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ValueApplied();
        }

        private void ValueApplied()
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            ValueApplied();
        }

        public bool IsOpen()
        {
            return (this == null ? false : true);
        }
    }
}
