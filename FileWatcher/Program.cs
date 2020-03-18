using System;
using System.IO;

namespace FileWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnHandlerEx;

            if (args.Length <= 0)
            {
                Console.WriteLine("parameter path not set");
                return;
            }

            string path = args[0];

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"{path} not exist");
                return;
            }

            Console.WriteLine("FileWatcher");
            Console.WriteLine($"set watch path: {path}");
            Console.WriteLine("Waiting activity ...");

            var fsw = new FileSystemWatcher
            {
                IncludeSubdirectories = true,
                Path = path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName
            };

            fsw.Created += watcher_Created;
            fsw.Deleted += watcher_Deleted;
            fsw.Changed += watcher_Changed;
            fsw.Renamed += watcher_Renamed;

            fsw.EnableRaisingEvents = true;

            Console.ReadLine();
        }

        private static void watcher_Renamed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File renamed - old name - {0}", e.Name);
        }

        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File changed - {0}, change type - {1}", e.Name, e.ChangeType);
        }

        private static void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File deleted - {0}", e.Name);
        }

        private static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created - {0}", e.Name);
        }

        private static void UnHandlerEx(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            Console.WriteLine("In app error. Application exit");
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    }
}
