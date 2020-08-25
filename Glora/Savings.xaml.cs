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

namespace Glora
{
    /// <summary>
    /// Interaction logic for Savings.xaml
    /// </summary>
    public partial class Savings : Window
    {
        public Savings()
        {
            InitializeComponent();
        }

        private void OnWindowMouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(windo);
            ell1.Width = 50 + (p.X / 30);
            ell2.Width = 50 + (p.X / 30);
            ell3.Width = 100 + (p.X / 30);
            ell4.Width = 100 + (p.X / 30);

            ell1.Height = 70 + (p.Y / 30);
            ell2.Height = 40 - (p.Y / 30);
            ell3.Height = 70 - (p.Y / 30);
            ell4.Height = 130 + (p.Y / 30);
        }
    }
}
