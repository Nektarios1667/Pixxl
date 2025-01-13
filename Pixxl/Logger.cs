using System;
using System.Collections.Generic;
using System.IO;

namespace Pixxl
{
    public static class Logger
    {
        public static List<string> logged = [];
        static Logger()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            File.WriteAllText("Logs/Log.txt", "");
            Log("Initialized Logger");
        }
        public static void Log(params string[] texts)
        {
            foreach (string text in texts)
            {
                logged.Add(text);
                File.AppendAllText("Logs/Log.txt", $"[{DateTime.Now:HH:mm:ss}] {text}\n");
            }
        }
    }
}
