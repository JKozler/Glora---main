using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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
    /// Interaction logic for Pripominky.xaml
    /// </summary>
    public partial class Pripominky : Window
    {
        public Pripominky()
        {
            InitializeComponent();
            RenderCreated();
        }

        public void RenderCreated()
        {
            if (File.Exists("remind.txt"))
            {
                using (StreamReader sr = new StreamReader("remind.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lblRemind.Items.Add(line);
                    }
                }
            }
            else { }
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            if (lblRemind.SelectedItem.ToString() != null)
            {
                using (StreamReader sr = new StreamReader(lblRemind.SelectedItem.ToString()))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        txtRemind.Text += line;
                    }
                }
            }
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            otherEdit.IsEnabled = true;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileName.Text != null)
            {
                Stream stream = new FileStream("remind.txt", FileMode.Append);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(fileName.Text);
                }
                Stream stream1 = new FileStream(fileName.Text, FileMode.Append);
                using (StreamWriter sw = new StreamWriter(stream1))
                {
                    sw.WriteLine(txtRemind.Text);
                }
            }
            else
                MessageBox.Show("Enter name.", "Missing name.");
        }
    }
}
