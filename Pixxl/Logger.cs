using System;
using System.Collections.Generic;
using System.IO;

namespace Pixxl
{
    public static class Logger
    {
        public static List<string> logged = [];
        private static string timestamp = DateTime.Now.ToString("MM-dd-yy_HH-mm-ss");
        private static string filename = $"log-{timestamp}.txt";
        static Logger()
        {
            // Loading
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            File.WriteAllText($"Logs/{filename}", "");
            Log("Initialized Logger");

            // Clearing old logs
            foreach (string file in Directory.GetFiles("Logs", "log-*.txt"))
            {
                string[] parts = Path.GetFileNameWithoutExtension(file).Split('-');

                if (parts.Length > 2 && int.TryParse(parts[1], out int day) && Math.Abs(day - DateTime.Now.Day) > 2)
                {
                    File.Delete(file);
                }
            }
        }
        public static void Log(params string[] texts)
        {
            foreach (string text in texts)
            {
                logged.Add(text);
                File.AppendAllText($"Logs/{filename}", $"[{DateTime.Now:HH:mm:ss}] {text}\n");
            }
        }
    }
}
