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
        string name = "";
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
                otherEdit.IsEnabled = true;
                name = lblRemind.SelectedItem.ToString();
                fileName.Text = lblRemind.SelectedItem.ToString();
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
            List<string> projects = new List<string>();
            if (lblRemind.SelectedItem.ToString() != null)
            {
                File.Delete(lblRemind.SelectedItem.ToString());
                if (File.Exists("remind.txt"))
                {
                    using (StreamReader sr = new StreamReader("remind.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string s = line + ".obr";
                            if (s == lblRemind.SelectedItem.ToString())
                            { }
                            else
                            {
                                projects.Add(line);
                            }
                        }
                    }
                }
                Stream stream = new FileStream("remind.txt", FileMode.Create);
                lblRemind.Items.Clear();
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach (string item in projects)
                    {
                        lblRemind.Items.Add(item + ".obr");
                        sw.WriteLine(item);
                    }
                }
            }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileName.Text != null)
            {
                if (name == fileName.Text){}
                else
                {
                    Stream stream = new FileStream("remind.txt", FileMode.Append);
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.WriteLine(fileName.Text);
                    }
                }
                Stream stream1 = new FileStream(fileName.Text, FileMode.Create);
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
