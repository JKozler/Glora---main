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
        public MainWindow()
        {
            InitializeComponent();
            ss.SpeakAsync("Welcome back.");
            
            //ss.SelectVoiceByHints(VoiceGender.Male);
        }

        private void speakBtn_Click(object sender, RoutedEventArgs e)
        {
            choice.Add(new string[] {"hi", "hello", "how are you",  "what is the current time", "open google", "thank you", "close", "cola"});
            DoubleAnimation doubleAnimation = new DoubleAnimation(15, 100, new Duration(new TimeSpan(0,0,3)));
            ellSpeak.BeginAnimation(WidthProperty, doubleAnimation);
            ellSpeak.BeginAnimation(HeightProperty, doubleAnimation);
            Grammar grammar = new Grammar(new GrammarBuilder(choice));
            try
            {
                sre.RequestRecognizerUpdate();
                sre.LoadGrammar(grammar);
                sre.SpeechRecognized += Sre_SpeechRecognized;
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            switch (e.Result.Text.ToString())
            {
                case "hello":
                    ss.SpeakAsync("hello sir");
                    break;

                case "how are you":
                    ss.SpeakAsync("really nice, and you?");
                    break;

                case "what is the current time":
                    ss.SpeakAsync("current time is " + DateTime.Now.ToLongDateString());
                    break;

                case "thank you":
                    ss.SpeakAsync("pleasure is on my side sir");
                    break;

                case "open google":
                    ss.SpeakAsync("Okey sir");
                    Process.Start("chrome", "https://www.google.com");
                    break;

                case "close":
                    this.Close();
                    break;

                default:
                    ss.SpeakAsync("iam searching for it sir!");
                    Process.Start("chrome", "https://www.google.com/search?q=" + e.Result.Text.ToString());
                    break;
            }
        }
    }
}
