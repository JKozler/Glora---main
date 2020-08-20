using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Glora;
using System.Text;
using System.Threading.Tasks;

namespace Glora
{
    class FileWatch
    {
        public static void Run()
        {
            string[] args = Environment.GetCommandLineArgs();
            args[0] = "E:\\";
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

                bool s = false;
                while (s == false) ;
            }
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
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
        private static void OnRenamed(object source, RenamedEventArgs e)
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
    }
}
