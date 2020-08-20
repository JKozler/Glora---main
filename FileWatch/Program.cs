using System;
using System.IO;
using System.Security.Permissions;

namespace FileWatch
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
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

                Console.WriteLine("Press 'q' and ENTER to quit.");
                while (Console.Read() != 'q') ;
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