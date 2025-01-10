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
            File.WriteAllText("Logs/Log.txt", "");
            Log("Initialized Logger");
        }
        public static void Log(params string[] texts)
        {
            foreach (string text in texts)
            {
                File.AppendAllText("Logs/Log.txt", $"[{DateTime.Now.ToString("HH:mm:ss")}] {text}\n");
            }
        }
    }
}
