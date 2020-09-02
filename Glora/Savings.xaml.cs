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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Shapes;
using RestSharp;

namespace Glora
{
    /// <summary>
    /// Interaction logic for Savings.xaml
    /// </summary>
    public partial class Savings : Window
    {
        int slary = 0;
        int expeness = 0;
        string currType = "";
        string perHour = "";
        string workingHours = "";
        string sa = "";
        string ex = "";
        public Savings()
        {
            InitializeComponent();
            cbCurrency.Items.Add("CZK");
            cbCurrency.Items.Add("DOL");
            cbCurrency.Items.Add("EUR");

            salarType.Items.Add("Per hour");
            salarType.Items.Add("Per day");
            salarType.Items.Add("Per month");
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

        private void calculateEarnings_Click(object sender, RoutedEventArgs e)
        {
            if (salary.Text != null && cbCurrency.SelectedItem != null && salarType.SelectedItem != null && tbExpeness.Text != null)
            {
                if (salarType.SelectedItem.ToString() == "Per hour")
                {
                    if (tbWorkingHour.Text != null)
                    {
                        sa = salary.Text;
                        ex = tbExpeness.Text;
                        currType = cbCurrency.SelectedItem.ToString();
                        perHour = salarType.SelectedItem.ToString();
                        workingHours = tbWorkingHour.Text;

                        int yearSalary = 0;
                        int expeness = 0;

                        yearSalary = CalculateErnings(perHour, Convert.ToInt32(sa), Convert.ToInt32(workingHours));
                        expeness = CalculateExpeness(Convert.ToInt32(ex));
                        int realOwn = yearSalary - expeness;

                        result.Text = "With salary " + sa + " (" + currType + ") " + perHour + " you are erning " + yearSalary.ToString() + "(" + currType + ") per year. But infact, your expeness will be " + expeness + "(" + currType + ") , so your real own money is " + realOwn + "(" + currType + ") per year.";

                    }
                    else
                        MessageBox.Show("YOu have to fill up everything.", "Type error");
                }
                else if (salarType.SelectedItem.ToString() == "Per day")
                {
                    sa = salary.Text;
                    ex = tbExpeness.Text;
                    currType = cbCurrency.SelectedItem.ToString();
                    perHour = salarType.SelectedItem.ToString();
                    workingHours = tbWorkingHour.Text;

                    int yearSalary = 0;
                    int expeness = 0;

                    yearSalary = CalculateErnings(perHour, Convert.ToInt32(sa), Convert.ToInt32(workingHours));
                    expeness = CalculateExpeness(Convert.ToInt32(ex));
                    int realOwn = yearSalary - expeness;

                    result.Text = "With salary " + sa + " (" + currType + ") " + perHour + " you are erning " + yearSalary.ToString() + "(" + currType + ") per year. But infact, your expeness will be " + expeness + "(" + currType + ") , so your real own money is " + realOwn + "(" + currType + ") per year.";

                }
                else
                {
                    sa = salary.Text;
                    ex = tbExpeness.Text;
                    currType = cbCurrency.SelectedItem.ToString();
                    perHour = salarType.SelectedItem.ToString();
                    workingHours = "0";

                    int yearSalary = 0;
                    int expeness = 0;

                    yearSalary = CalculateErnings(perHour, Convert.ToInt32(sa), Convert.ToInt32(workingHours));
                    expeness = CalculateExpeness(Convert.ToInt32(ex));
                    int realOwn = yearSalary - expeness;

                    result.Text = "With salary " + sa + " (" + currType + ") " + perHour + " you are erning " + yearSalary.ToString() + "(" + currType + ") per year. But infact, your expeness will be " + expeness + "(" + currType + ") , so your real own money is " + realOwn + "(" + currType + ") per year." + ActualPriceOf();
                    

                }
            }
            else
                MessageBox.Show("YOu have to fill up everything.", "Type error");
        }

        private void salarType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (salarType.SelectedItem.ToString() == "Per hour")
            {
                lblWorking.Visibility = Visibility.Visible;
                lblWorking.Content = "How many hours per week you working? ";
                tbWorkingHour.Visibility = Visibility.Visible;
            }
            else if (salarType.SelectedItem.ToString() == "Per day")
            {
                lblWorking.Visibility = Visibility.Visible;
                lblWorking.Content = "How many days per week are you working? ";
                tbWorkingHour.Visibility = Visibility.Visible;
            }
            else if (salarType.SelectedItem.ToString() == "Per month")
            {
                lblWorking.Visibility = Visibility.Hidden;
                tbWorkingHour.Visibility = Visibility.Hidden;
            }
        }

        public int CalculateErnings(string type, int salary, int hours)
        {
            int res = 0;

            if (type == "Per hour")
                res = hours * 4 * 12 * salary;
            else if (type == "Per day")
                res = salary * hours * 4 * 12;
            else
                res = salary * 12;

            return res;
        }
        public int CalculateExpeness(int expeness)
        {
            return expeness * 12;
        }
        public string ActualPriceOf()
        {
            var client = new RestClient("https://www.goldapi.io/api/XAU/USD");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-access-token", "goldapi-3z1wxykee9avny-io");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
