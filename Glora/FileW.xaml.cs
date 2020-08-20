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
using System.Speech.Synthesis;
using System.IO;
using System.Threading;

namespace Glora
{
    /// <summary>
    /// Interaction logic for FileW.xaml
    /// </summary>
    public partial class FileW : Window
    {
        bool s = true;
        SpeechSynthesizer ss = new SpeechSynthesizer();
        public FileW()
        {
            InitializeComponent();
            ss.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Senior);
        }

        public void Start()
        {
            ss.SpeakAsync("I am closing myself and I am watching your computer.");
            string[] args = Environment.GetCommandLineArgs();
            args[0] = "E:\\";
            Thread.Sleep(1000);
            using (FileSystemWatcher fsw = new FileSystemWatcher())
            {
                fsw.Path = args[0];
                fsw.NotifyFilter = NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.FileName
                                | NotifyFilters.DirectoryName;

                fsw.Renamed += OnRenamed;
                fsw.Created += OnChanged;
                fsw.Deleted += OnChanged;
                fsw.Changed += OnChanged;

                fsw.EnableRaisingEvents = true;

                while (s == false) ;
            }
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            if (File.Exists("history.txt"))
            {
                using (StreamWriter sw = new StreamWriter("history.txt"))
                {
                    sw.WriteLine("File " + e.FullPath + " " + e.ChangeType);
                }
            }
            else
            {
                Stream stream = new FileStream("history.txt", FileMode.Append);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine("File " + e.FullPath + " " + e.ChangeType);
                }
            }
        }
        public void OnRenamed(object source, RenamedEventArgs e)
        {
            if (File.Exists("history.txt"))
            {
                using (StreamWriter sw = new StreamWriter("history.txt"))
                {
                    sw.WriteLine("File was renamed form " + e.OldFullPath + " to " + e.FullPath);
                }
            }
            else
            {
                Stream stream = new FileStream("history.txt", FileMode.Append);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine("File was renamed form " + e.OldFullPath + " to " + e.FullPath);
                }
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            Start();
            s = false;
        }
    }
}
