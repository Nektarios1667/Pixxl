using System;
using System.IO;

namespace Pixxl
{
    public static class Logger
    {
        static Logger()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            Log("Logger initialized");
        }
        public static void Log(string text)
        {
            File.WriteAllText($"Logs/Log.txt", $"[{DateTime.Now.ToString("HH:mm:ss")}] {text}\n");
        }
    }
}
