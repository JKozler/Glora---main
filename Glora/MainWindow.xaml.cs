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
using System.IO;
using GoogleApi;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Net;

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
        string helpSearch = "";
        bool speak = false;
        string name;
        bool anim = true;
        string[] motivate = { "The harder you work for something, the greater you’ll feel when you achieve it.", "Don’t stop when you’re tired. Stop when you’re done.", "Do something today that your future self will thank you for.", " Sometimes we’re tested not to show our weaknesses, but to discover our strengths.", "The key to success is to focus on goals, not obstacles.", "The pessimist sees difficulty in every opportunity. The optimist sees opportunity in every difficulty.", "It’s not whether you get knocked down, it’s whether you get up.", "If you have something to do today, don't do it and you will have one free day." };
        public MainWindow()
        {
            InitializeComponent();
            ss.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Senior);
            name = "jackob";
            wind.WindowState = WindowState.Maximized;
            //wind.Height = SystemParameters.PrimaryScreenHeight;
            ss.SpeakAsync("Welcome back.");
            
            //ss.SelectVoiceByHints(VoiceGender.Male);
        }

        private void speakBtn_Click(object sender, RoutedEventArgs e)
        {
            if (speak == false)
            {
                using (StreamReader sr = new StreamReader("commands.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        choice.Add(line);
                    }
                }
                //choice.Add(new string[] { "hi", "hello", "hello Glora", "hi Glora", "hello my girl", "how are you", "browser", "web", "what is the current date", "open google", "thank you", "close", "cola", "open youtube", "search youtube", "are you here" });
                DoubleAnimation doubleAnimation = new DoubleAnimation(15, 100, new Duration(new TimeSpan(0, 0, 3)));
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
                catch (Exception)
                {
                    //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                speak = true;
            }
            else
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation(100, 15, new Duration(new TimeSpan(0, 0, 3)));
                ellSpeak.BeginAnimation(WidthProperty, doubleAnimation);
                ellSpeak.BeginAnimation(HeightProperty, doubleAnimation);
                sre.RecognizeAsyncStop();
                speak = false;
            }
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            while (anim)
            {
                DoubleAnimation ani = new DoubleAnimation(0, 75, new Duration(new TimeSpan(0, 0, 3)));
                ellGlora.BeginAnimation(WidthProperty, ani);
                ellGlora.BeginAnimation(HeightProperty, ani);
                anim = false;
            }
            string s = e.Result.Text.ToString();
            if (s.ToLower().Contains("hello there"))
            {
                ss.SpeakAsync("General Kenobi!");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("General Kenobi!");
            }
            else if (s.ToLower().Contains("hello") || s.ToLower().Contains("hi"))
            {
                ss.SpeakAsync("hello sir");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("Hello sir!");
            }
            else if (s.ToLower().Contains("current date"))
            {
                ss.SpeakAsync("current date is " + DateTime.Now.ToLongDateString());
                gloraSay.Items.Add("");
                gloraSay.Items.Add(DateTime.Now.ToLongDateString());
            }
            else if (s.ToLower().Contains("how old") && s.ToLower().Contains("you"))
            {
                ss.SpeakAsync("It is not polit to ask about woman's age");
                gloraSay.Items.Add("It is not polit to ask about woman's age.");
            }
            else if (s.ToLower().Contains("time"))
            {
                ss.SpeakAsync("current date is " + DateTime.Now.ToLocalTime());
                gloraSay.Items.Add("");
                gloraSay.Items.Add(DateTime.Now.ToLocalTime().ToString());
            }
            else if (s.ToLower().Contains("10 to 0"))
            {
                ss.SpeakAsync("Ok sir!");
                ss.SpeakAsync("10!"); ss.SpeakAsync("9!"); ss.SpeakAsync("8!"); ss.SpeakAsync("7!"); ss.SpeakAsync("6!"); ss.SpeakAsync("5!"); ss.SpeakAsync("4!"); ss.SpeakAsync("3!"); ss.SpeakAsync("2!"); ss.SpeakAsync("1!"); ss.SpeakAsync("Misík!");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("10,9,8....");
            }
            else if (s.ToLower().Contains("are you here"))
            {
                ss.SpeakAsync("I am here sir, dont be afraid");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("i am here sir, don't be afraid!");
            }
            else if (s.ToLower().Contains("fine"))
            {
                ss.SpeakAsync("I am glad.");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("I am glad!");
            }
            else if (s.ToLower().Contains("motivate me"))
            {
                ss.SpeakAsync("Lets do it!");
                Random random = new Random();
                int y = random.Next(0,7);
                ss.SpeakAsync(motivate[y]);
                gloraSay.Items.Add("");
                gloraSay.Items.Add("Let's do it!");
            }
            else if (s.ToLower().Contains("how are you"))
            {
                ss.SpeakAsync("Thanks for asking sir, i am fine, and you?");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("Thanks for asking sir, I am fine, and you?");
            }
            else if (s.ToLower().Contains("thank you"))
            {
                ss.SpeakAsync("pleasure is on my side " + name);
                gloraSay.Items.Add("");
                gloraSay.Items.Add("Pleasure is on my side" + name);
            }
            else if (s.ToLower().Contains("browser") || s.ToLower().Contains("web") || s.ToLower().Contains("google"))
            {
                ss.SpeakAsync("I am opening google sir!");
                Process.Start("chrome", "https://www.google.com");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("I am opening google sir!");
            }
            else if (s.ToLower().Contains("close"))
            {
                ss.Speak("See you later!");
                this.Close();
            }
            else if (s.ToLower().Contains("calculator"))
            {
                ss.Speak("opening calculator!");
                Process.Start("calculator");
            }
            else if (s.ToLower().Contains("youtube"))
            {
                ss.SpeakAsync("Opening YouTube sir");
                Process.Start("chrome", "https://www.youtube.com");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("Opening YouTube sir!");
            }
            else if (s.ToLower().Contains("search "))
            {
                string urlAddress = "https://www.google.co.il/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8#q=what%20is%20the%20weight%20of%20human%20heart";
                // need to process to get the real URL of the question.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    Stream stream = new FileStream("web.txt", FileMode.Create);
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.WriteLine(readStream.ReadToEnd());
                    }
                    response.Close();
                    readStream.Close();
                }

                string g = s.Replace(' ', '+');
                webPageCode.IsEnabled = true;
                ss.SpeakAsync("Okay sir, I am searching for it");
                int y = g.IndexOf("f") + 4;
                search = g.Substring(y);

                Process.Start("chrome", "https://www.google.com/search?q=" + search);
            }

            DoubleAnimation ani2 = new DoubleAnimation(75, 0, new Duration(new TimeSpan(0, 0, 3)));
            ellGlora.BeginAnimation(WidthProperty, ani2);
            ellGlora.BeginAnimation(HeightProperty, ani2);
            dictateTb.Items.Add("You: " + s);
            dictateTb.Items.Add(" ");
        }

        private void dictateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tbCommandForPeople.Text != "")
            {
                if (tbCommandForPeople.Text.ToLower().Contains("hello there"))
                {
                    ss.SpeakAsync("General Kenobi!");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("General Kenobi!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("hello") || tbCommandForPeople.Text.ToLower().Contains("hi"))
                {
                    ss.SpeakAsync("hello sir");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Hello sir!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("time"))
                {
                    ss.SpeakAsync("current date is " + DateTime.Now.ToLocalTime());
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add(DateTime.Now.ToLocalTime().ToString());
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("how old") && tbCommandForPeople.Text.ToLower().Contains("you"))
                {
                    ss.SpeakAsync("It is not polit to ask about woman's age");
                    gloraSay.Items.Add("It is not polit to ask about woman's age.");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("current date"))
                {
                    ss.SpeakAsync("current date is " + DateTime.Now.ToLongDateString());
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add(DateTime.Now.ToLongDateString());
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("are you here"))
                {
                    ss.SpeakAsync("I am here sir, dont be afraid");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("I am here sir, don't be afraid!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("motivate me"))
                {
                    ss.SpeakAsync("Lets do it!");
                    Random random = new Random();
                    int y = random.Next(0, 7);
                    ss.SpeakAsync(motivate[y]);
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Let's do it!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("fine"))
                {
                    ss.SpeakAsync("I am glad.");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("I am glad!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("10 to 0"))
                {
                    ss.SpeakAsync("Ok sir!");
                    ss.SpeakAsync("10!"); ss.SpeakAsync("9!"); ss.SpeakAsync("8!"); ss.SpeakAsync("7!"); ss.SpeakAsync("6!"); ss.SpeakAsync("5!"); ss.SpeakAsync("4!"); ss.SpeakAsync("3!"); ss.SpeakAsync("2!"); ss.SpeakAsync("1!"); ss.SpeakAsync("Misík!");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("10,9,8....");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("how are you"))
                {
                    ss.SpeakAsync("Thanks for asking sir, i am fine, and you?");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Thanks for asking, I am fine, and you?");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("thank you"))
                {
                    ss.SpeakAsync("pleasure is on my side " + name);
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Pleasure is on my side " + name);
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("browser") || tbCommandForPeople.Text.ToLower().Contains("web") || tbCommandForPeople.Text.ToLower().Contains("google"))
                {
                    ss.SpeakAsync("I am opening google sir!");
                    Process.Start("chrome", "https://www.google.com");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("I am opening google sir!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("close"))
                {
                    ss.Speak("See you later!");
                    this.Close();
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("calculator"))
                {
                    ss.Speak("openning calculator");
                    //Process.Start("calculator");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains(".cz") || tbCommandForPeople.Text.ToLower().Contains(".com"))
                {
                    ss.Speak("I am looking for that page " + tbCommandForPeople.Text);

                    string urlAddress = "https://www." + tbCommandForPeople.Text;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;

                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }
                        Stream stream = new FileStream("web.txt", FileMode.Create);
                        using (StreamWriter sw = new StreamWriter(stream))
                        {
                            sw.WriteLine(readStream.ReadToEnd());
                        }
                        response.Close();
                        readStream.Close();
                    }

                    Process.Start("chrome", "https://www." + tbCommandForPeople.Text);
                    webPageCode.IsEnabled = true;
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("youtube"))
                {
                    ss.SpeakAsync("Opening YouTube sir");
                    Process.Start("chrome", "https://www.youtube.com");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Opening YouTube sir!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("search "))
                {
                    string g = tbCommandForPeople.Text.Replace(' ', '+');
                    ss.SpeakAsync("Okay sir, I am searching for it");
                    int y = g.IndexOf("f") + 4;
                    search = g.Substring(y);

                    string urlAddress = "https://www.google.com/search?q=" + search;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;

                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        Stream stream = new FileStream("web.txt", FileMode.Create);
                        using (StreamWriter sw = new StreamWriter(stream))
                        {
                            sw.WriteLine(readStream.ReadToEnd());
                        }
                        response.Close();
                        readStream.Close();
                    }
                    webPageCode.IsEnabled = true;

                    Process.Start("chrome", "https://www.google.com/search?q=" + search);
                }

                DoubleAnimation ani2 = new DoubleAnimation(75, 0, new Duration(new TimeSpan(0, 0, 3)));
                ellGlora.BeginAnimation(WidthProperty, ani2);
                ellGlora.BeginAnimation(HeightProperty, ani2);
                dictateTb.Items.Add("You: " + tbCommandForPeople.Text);
                dictateTb.Items.Add(" ");
            }
            else{}
            tbCommandForPeople.Text = "";
        }

        private void tbCommandForPeople_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dictateBtn_Click(sender, e);
            }
        }

        private void webPageCode_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("web.txt");
        }
    }
}
