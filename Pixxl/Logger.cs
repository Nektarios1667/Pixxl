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
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            File.WriteAllText($"Logs/{filename}", "");
            Log("Initialized Logger");
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
