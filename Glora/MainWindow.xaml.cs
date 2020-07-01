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
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace Glora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechSynthesizer ss = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        Choices choice = new Choices();
        string search = "";

        public MainWindow()
        {
            InitializeComponent();
            //wind.Height = SystemParameters.PrimaryScreenHeight;
            ss.SpeakAsync("Welcome back.");
            
            //ss.SelectVoiceByHints(VoiceGender.Male);
        }

        private void speakBtn_Click(object sender, RoutedEventArgs e)
        {
            choice.Add(new string[] {"hi", "hello", "how are you",  "what is the current date", "open google", "thank you", "close", "cola"});
            DoubleAnimation doubleAnimation = new DoubleAnimation(15, 100, new Duration(new TimeSpan(0,0,3)));
            ellSpeak.BeginAnimation(WidthProperty, doubleAnimation);
            ellSpeak.BeginAnimation(HeightProperty, doubleAnimation);
            Grammar grammar = new Grammar(new GrammarBuilder(choice));
            try
            {
                sre.RequestRecognizerUpdate();
                sre.LoadGrammar(new DictationGrammar());
                sre.SpeechRecognized += Sre_SpeechRecognized;
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string s = e.Result.Text.ToString();
            if (s.ToLower().Contains("hello"))
                ss.SpeakAsync("hello sir");
            else if (s.ToLower().Contains("current date"))
                ss.SpeakAsync("current date is " + DateTime.Now.ToLongDateString());
            else if (s.ToLower().Contains("are you here"))
                ss.SpeakAsync("I am here sir, dont be afraid");
            else if (s.ToLower().Contains("browser") || s.ToLower().Contains("web"))
            {
                ss.SpeakAsync("I am opening google sir!");
                Process.Start("chrome", "https://www.google.com");
            }
            else if (s.ToLower().Contains("videos"))
            {
                ss.SpeakAsync("Opening YT sir");
                Process.Start("chrome", "https://www.youtube.com");
            }
            else if (s.ToLower().Contains("search "))
            {
                ss.SpeakAsync("Okay sir, I am searching for it");
                char[] z = s.ToCharArray();
                int y = 0;
                for (int i = 0; i < z.Length; i++)
                {
                    if (z[i] == 's' && z[i + 1] == 'e' && z[i + 2] == 'a' && z[i + 3] == 'r' && z[i + 4] == 'c' && z[i + 5] == 'h' && z[i + 6] == ' ')
                    {
                        if ((i+7) == z.Length - 1)
                        {
                            ss.SpeakAsync("What i have to serach for sir?");
                        }
                        else if (z[i + 7] == 'f' && z[i + 8] == 'o' && z[i + 9] == 'r' && z[i + 10] == ' ')
                        {
                            y = i + 11;
                            for (int g = 0; g <= (z.Length - 1 - y); g++)
                            {
                                search += z[y];
                                y++;
                            }
                        }
                        else
                        {
                            y = i + 7;
                            for (int g = 0; g <= (z.Length - 1 - y); g++)
                            {
                                search += z[y];
                                y++;
                            }
                        }
                       
                    }
                }
               

            }
            if (search != "")
            {
                label.Content = search;
                Process.Start("chrome", "https://www.google.com/search?q=" + search);
                search = "";
            }
            dictateBtn.Content = s;
            //switch (e.Result.Text.ToString())
            //{
            //    case "hello":
            //        ss.SpeakAsync("hello sir");
            //        break;

            //    case "how are you":
            //        ss.SpeakAsync("really nice, and you?");
            //        break;

            //    case "what is the current date":
            //        ss.SpeakAsync("current date is " + DateTime.Now.ToLongDateString());
            //        break;

            //    case "thank you":
            //        ss.SpeakAsync("pleasure is on my side sir");
            //        break;

            //    case "open google":
            //        ss.SpeakAsync("Okey sir");
            //        Process.Start("chrome", "https://www.google.com");
            //        break;

            //    case "close":
            //        this.Close();
            //        break;

            //    default:
            //        ss.SpeakAsync("iam searching for it sir!");
            //        Process.Start("chrome", "https://www.google.com/search?q=" + e.Result.Text.ToString());
            //        break;
            //}
        }

        private void dictateBtn_Click(object sender, RoutedEventArgs e)
        {
            ss.SpeakAsync("Youcan speak and all what you want will be write in text box above you.");

        }
    }
}
