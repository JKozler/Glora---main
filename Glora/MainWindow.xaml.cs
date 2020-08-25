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
using Microsoft.Win32;
using MaterialDesignColors.Recommended;

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
        string path = "";
        string allTxt = "";
        string helpSearch = "";
        bool speak = false;
        bool cantOpen = false;
        bool problems = false;
        bool problemsHelp = false;
        string name;
        bool anim = true;
        string[] motivate = { "The harder you work for something, the greater you’ll feel when you achieve it.", "Don’t stop when you’re tired. Stop when you’re done.", "Do something today that your future self will thank you for.", " Sometimes we’re tested not to show our weaknesses, but to discover our strengths.", "The key to success is to focus on goals, not obstacles.", "The pessimist sees difficulty in every opportunity. The optimist sees opportunity in every difficulty.", "It’s not whether you get knocked down, it’s whether you get up.", "If you have something to do today, don't do it and you will have one free day." };
        string[] greetings = { "Hi sir", "Hi", "Hello, how are you.", "I am really glad to see you again", "Hey", "Whoops, you think it was lag, nope, it was prank...", "Whats up sir.", "Welcome" };
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
            else if (s.ToLower().Contains("hello") || s.ToLower().Contains("hi") || s.ToLower().Contains("hey") || s.ToLower().Contains("glora"))
            {
                Random random = new Random();
                int x = random.Next(0, 7);
                if (greetings[x] == "Whoops, you think it was lag, nope, it was prank...")
                    Thread.Sleep(2500);
                ss.SpeakAsync(greetings[x]);
                gloraSay.Items.Add("");
                gloraSay.Items.Add(greetings[x]);
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
            else if (s.ToLower().Contains("from") && s.ToLower().Contains("to"))
            {
                int y = s.IndexOf("m") + 2;
                string se = s.Substring(y);
                int y2 = se.IndexOf(" ") - 1;
                int y3 = se.IndexOf(" ") + 1;
                string re = se.Substring(y3);
                int x1 = Convert.ToInt32(se.Substring(0, y2));
                int x2 = Convert.ToInt32(re);
                ss.SpeakAsync("Ok sir!");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("Counting...");
                while (x1 != x2)
                {
                    ss.SpeakAsync(x1.ToString());
                    x1--;
                }
                if (x1 == x2)
                {
                    ss.SpeakAsync("DONE!!!");
                }
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
            else if (s.ToLower().Contains("thank"))
            {
                ss.SpeakAsync("I am trying to do my best sir!");
                gloraSay.Items.Add("");
                gloraSay.Items.Add("I am trying to do my best sir!");
            }
            else if (s.ToLower().Contains("minimaze"))
            {
                ss.SpeakAsync("I am minimazing myself!");
                wind.WindowState = WindowState.Minimized;
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
            else if (s.ToLower().Contains("reset"))
            {
                ss.SpeakAsync("I am reseting myself.?");
                tbFileOpenInfo.Clear();
                tbFileOpenInfo.IsEnabled = false;
                btnSaveReadingFile.IsEnabled = false;
                gloraSay.Items.Clear();
                dictateTb.Items.Clear();
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
                if (problems == true)
                {
                    if (cantOpen == true)
                    {
                    }
                    else if (tbCommandForPeople.Text.ToLower().Contains("processor") || tbCommandForPeople.Text.ToLower().Contains("lag") || tbCommandForPeople.Text.ToLower().Contains("bug"))
                    {
                        problems = false;
                        ss.SpeakAsync("I prefer restart PC, if this not work, you can upgrade your computer. Can I restart your machine?");
                        gloraSay.Items.Add("");
                        gloraSay.Items.Add("Can I restart your machine? <yes/no>");
                        problemsHelp = true;
                    }
                    else if (tbCommandForPeople.Text.ToLower().Contains("program") || tbCommandForPeople.Text.ToLower().Contains("open"))
                    {
                        cantOpen = true;
                        ss.SpeakAsync("Please write exact name of the progrma or file.");
                        gloraSay.Items.Add("");
                        gloraSay.Items.Add("Please write exact name of the progrma/file.");
                    }
                    else if (tbCommandForPeople.Text.ToLower().Contains("done"))
                    {
                        problems = false;
                        ss.SpeakAsync("Okay, I believe i help you fine.");
                    }
                    else if (tbCommandForPeople.Text.ToLower().Contains("yes") || tbCommandForPeople.Text.ToLower().Contains("jop") || tbCommandForPeople.Text.ToLower().Contains("of course") || tbCommandForPeople.Text.ToLower().Contains("can do"))
                    {
                        Process.Start("shutdown.exe", "-r");
                        ss.SpeakAsync("Ok, please wait little moment.");
                    }
                }
                if (tbCommandForPeople.Text.ToLower().Contains("have") && tbCommandForPeople.Text.ToLower().Contains("problem"))
                {
                    problems = true;
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Type something like lag/bug/memory/storage etc..");
                    ss.SpeakAsync("What kind of problem you have?");
                }
                if (tbCommandForPeople.Text.ToLower().Contains("saving program") || tbCommandForPeople.Text.ToLower().Contains("calculate my savings") || tbCommandForPeople.Text.ToLower().Contains("calculate saving"))
                {
                    ss.SpeakAsync("I am opening my special software for that, sir.");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Opening Savings...");
                    Savings savings = new Savings();
                    savings.Show();
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("hello there"))
                {
                    ss.SpeakAsync("General Kenobi!");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("General Kenobi!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("watch file"))
                {
                    wind.WindowState = WindowState.Minimized;
                    FileW fileW = new FileW();
                    fileW.ShowDialog();
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("read") || tbCommandForPeople.Text.ToLower().Contains("show"))
                {
                    tbFileOpenInfo.Clear();
                    allTxt = "";
                    ss.SpeakAsync("What kind of file I have to read?");
                    OpenFileDialog opf = new OpenFileDialog();
                    opf.Filter = "Text|*.txt|All|*.*";
                    Nullable<bool> res = opf.ShowDialog();
                    if (res == true)
                    {
                        path = opf.FileName;
                    }
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            ss.SpeakAsync(line);
                            allTxt += line + Environment.NewLine;
                        }
                        tbFileOpenInfo.Text = allTxt;
                    }
                    btnSaveReadingFile.IsEnabled = true;
                    tbFileOpenInfo.IsEnabled = true;
                    gloraSay.Items.Add("You can edit the file.");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("thank"))
                {
                    ss.SpeakAsync("I am trying to do my best sir!");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("I am trying to do my best sir!");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("minimaze"))
                {
                    ss.SpeakAsync("I am minimazing myself!");
                    wind.WindowState = WindowState.Minimized;
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("hello") || tbCommandForPeople.Text.ToLower().Contains("hi") || tbCommandForPeople.Text.ToLower().Contains("hey") || tbCommandForPeople.Text.ToLower().Contains("glora"))
                {
                    Random random = new Random();
                    int x = random.Next(0, 7);
                    if (greetings[x] == "Whoops, you think it was lag, nope, it was prank...")
                        Thread.Sleep(2500);
                    ss.SpeakAsync(greetings[x]);
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add(greetings[x]);
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
                else if (tbCommandForPeople.Text.ToLower().Contains("from") && tbCommandForPeople.Text.ToLower().Contains("to"))
                {
                    int y = tbCommandForPeople.Text.IndexOf("m") + 2;
                    string se = tbCommandForPeople.Text.Substring(y);
                    int y2 = se.IndexOf(" ");
                    int y3 = se.IndexOf(" ") + 1;
                    string se2 = se.Substring(y3);
                    int y4 = se2.IndexOf("o") + 2;
                    string se3 = se2.Substring(y4);
                    int x1 = Convert.ToInt32(se.Substring(0, y2));
                    int x2 = Convert.ToInt32(se3);
                    ss.SpeakAsync("Ok sir!");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Counting....");
                    while (x1 != x2)
                    {
                        ss.SpeakAsync(x1.ToString());
                        x1--;
                    }
                    if (x1 == x2)
                    {
                        ss.SpeakAsync("DONE!!!");
                    }
                }

                else if (tbCommandForPeople.Text.ToLower().Contains("how are you"))
                {
                    ss.SpeakAsync("Thanks for asking sir, i am fine, and you?");
                    gloraSay.Items.Add("");
                    gloraSay.Items.Add("Thanks for asking, I am fine, and you?");
                }
                else if (tbCommandForPeople.Text.ToLower().Contains("reset"))
                {
                    ss.SpeakAsync("I am reseting myself.?");
                    tbFileOpenInfo.Clear();
                    tbFileOpenInfo.IsEnabled = false;
                    btnSaveReadingFile.IsEnabled = false;
                    gloraSay.Items.Clear();
                    dictateTb.Items.Clear();
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

            if (problemsHelp == true)
            {
                problems = true;
            }
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

        private void btnSaveReadingFile_Click(object sender, RoutedEventArgs e)
        {
            Stream stream = new FileStream(path, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.WriteLine(tbFileOpenInfo.Text);
            }
            btnSaveReadingFile.IsEnabled = false;
            ss.SpeakAsync("File was saved successfully.");
        }

        private void tbFileOpenInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.FontDialog fd = new System.Windows.Forms.FontDialog();
            System.Windows.Forms.DialogResult dg = fd.ShowDialog();
            if (dg == System.Windows.Forms.DialogResult.OK)
            {
                tbFileOpenInfo.FontFamily = new FontFamily(fd.Font.Name);
                tbFileOpenInfo.FontSize = fd.Font.Size * 96.0 / 72.0;
                tbFileOpenInfo.FontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                tbFileOpenInfo.FontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                TextDecorationCollection tdc = new TextDecorationCollection();
                if (fd.Font.Underline) tdc.Add(TextDecorations.Underline);
                if (fd.Font.Strikeout) tdc.Add(TextDecorations.Strikethrough);
                tbFileOpenInfo.TextDecorations = tdc;
            }
        }

        private void pripominky_Click(object sender, RoutedEventArgs e)
        {
            ss.SpeakAsync("I am opening your hard thinkies.");
            Pripominky pripominky = new Pripominky();
            pripominky.Show();
        }
    }
}
