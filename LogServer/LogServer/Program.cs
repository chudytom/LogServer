using System;
using System.IO;

namespace LogServer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
                throw new ArgumentException("The pogram requires a single argument that is a valid file path");

            var filePath = args[0];
            var logManager = new LogManager();
            var logs = logManager.ReadLogsFromFile(filePath);

            foreach (var log in logs)
            {
                Console.WriteLine(log.Id);
            }
        }
    }
}
