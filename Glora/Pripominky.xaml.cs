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
            if (File.Exists("doneTasks.txt"))
            {
                using (StreamReader sr = new StreamReader("doneTasks.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lblDone.Items.Add(line);
                    }
                }
            }
            else { }
            if (File.Exists("missTasks.txt"))
            {
                using (StreamReader sr = new StreamReader("missTasks.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lblMiss.Items.Add(line);
                    }
                }
            }
            else { }
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            if (lblRemind.SelectedItem.ToString() != null)
            {
                txtRemind.Clear();
                fileName.Clear();
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
            txtRemind.Clear();
            fileName.Clear();
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
                            string s = line;
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
                        lblRemind.Items.Add(item + ".txt");
                        sw.WriteLine(item);
                    }
                }
                lblRemind.Items.Remove(lblRemind.SelectedItem);
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
                        sw.WriteLine(fileName.Text + ".txt");
                    }
                }
                Stream stream1 = new FileStream(fileName.Text + ".txt", FileMode.Create);
                using (StreamWriter sw = new StreamWriter(stream1))
                {
                    sw.WriteLine(txtRemind.Text);
                }
                lblRemind.Items.Add(fileName.Text + ".txt");
            }
            else
                MessageBox.Show("Enter name.", "Missing name.");
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            if (fileName.Text != null)
            {
                Stream stream = new FileStream("doneTasks.txt", FileMode.Append);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(fileName.Text);
                }

                List<string> projects = new List<string>();
                File.Delete(fileName.Text);
                if (File.Exists("remind.txt"))
                {
                    using (StreamReader sr = new StreamReader("remind.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string s = line;
                            if (s == fileName.Text)
                            { }
                            else
                            {
                                projects.Add(line);
                            }
                        }
                    }
                }
                Stream stream2 = new FileStream("remind.txt", FileMode.Create);
                lblRemind.Items.Clear();
                using (StreamWriter sw = new StreamWriter(stream2))
                {
                    foreach (string item in projects)
                    {
                        lblRemind.Items.Add(item + ".txt");
                        sw.WriteLine(item);
                    }
                }
                lblDone.Items.Add(fileName.Text);
            }
        }

        private void miss_Click(object sender, RoutedEventArgs e)
        {
            if (fileName.Text != null)
            {
                Stream stream = new FileStream("missTasks.txt", FileMode.Append);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(fileName.Text);
                }

                List<string> projects = new List<string>();
                File.Delete(fileName.Text);
                if (File.Exists("remind.txt"))
                {
                    using (StreamReader sr = new StreamReader("remind.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string s = line;
                            if (s == fileName.Text)
                            { }
                            else
                            {
                                projects.Add(line);
                            }
                        }
                    }
                }
                Stream stream2 = new FileStream("remind.txt", FileMode.Create);
                lblRemind.Items.Clear();
                using (StreamWriter sw = new StreamWriter(stream2))
                {
                    foreach (string item in projects)
                    {
                        lblRemind.Items.Add(item + ".txt");
                        sw.WriteLine(item);
                    }
                }
                lblMiss.Items.Add(fileName.Text);
            }
        }
    }
}
