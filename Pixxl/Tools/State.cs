using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Pixxl.Materials;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Tools
{
    public static class State
    {
        static State()
        {
            if (!Directory.Exists("Saves"))
            {
                Directory.CreateDirectory("Saves");
            }
            Logger.Log("State initialized");
        }
        public static void SaveCanvas(Canvas canvas, int saveNumber) { SavePixels(canvas.Pixels, saveNumber); }
        public static void ClearPixels(Canvas canvas, int saveNumber)
        {
            State.SavePixels(Canvas.Cleared(canvas), saveNumber);
        }
        public static void SavePixels(Pixel[] pixels, int saveNumber)
        {
            // Logging
            Logger.Log($"Saving to slot {saveNumber}...");

            // Thread
            Thread saveThread = new(() => Save(pixels, saveNumber));
            saveThread.Start();
        }
        private static void Save(Pixel[] pixels, int saveNumber)
        {
            // Data
            string data = $"{Constants.Screen.Grid[0]}x{Constants.Screen.Grid[1]}\n{Constants.Screen.PixelSize}\n";
            for (int i = 0; i < pixels.Length; i++)
            {
                Pixel current = pixels[i];
                data += $"{current.TypeId};{Math.Round(current.Temperature, 2)}\n";
            }

            //Create a GZipStream to compress the data while writing to the file
            using FileStream fileStream = new($"Saves/save-{saveNumber}.pxs", FileMode.Create, FileAccess.Write);
            using GZipStream gzipStream = new(fileStream, CompressionLevel.Optimal);
            using StreamWriter writer = new(gzipStream);
            writer.Write(data);

            // Logging
            Logger.Log($"Saved to slot {saveNumber}");
        }
        public static void LoadCanvas(Canvas canvas, int saveNumber)
        {
            // Logging
            Logger.Log($"Loading from slot {saveNumber}...");

            // Thread
            Thread saveThread = new(() => Load(canvas, saveNumber));
            saveThread.Start();
        }
        private static void Load(Canvas canvas, int saveNumber)
        {
            // Reading
            string[] lines;
            try
            {
                // Open the gzipped file
                using GZipStream gzipStream = new(new FileStream($"Saves/save-{saveNumber}.pxs", FileMode.Open), CompressionMode.Decompress);
                using StreamReader reader = new(gzipStream);
                // Read all lines from the gzipped file
                lines = reader.ReadToEnd().Split("\n");
            }
            catch (FileNotFoundException)
            {
                Logger.Log($"File 'save-{saveNumber}.pxs' not found"); return;
            }
            catch (Exception e)
            {
                Logger.Log($"Error reading file: {e}"); return;
            }
            // Metadata
            Xna.Vector2 grid;
            int pixelSize;
            try
            {
                grid = new(int.Parse(lines[0].Split("x")[0]), int.Parse(lines[0].Split("x")[1]));
                pixelSize = int.Parse(lines[1]);
            }
            catch (IndexOutOfRangeException) { Logger.Log("File header data corrupted or in wrong format"); return; }
            catch (FormatException) { Logger.Log("File header data corrupted or in wrong format"); return; }

            // Loading
            string line;
            string[] pixels = lines[2..];
            for (int l = 0; l < pixels.Length; l++)
            {
                line = pixels[l];
                if (line == "") { continue; }

                try
                {
                    string[] data = line.Split(';');
                    Pixel? created = Canvas.New(canvas, Registry.Materials.Names[int.Parse(data[0])], new(pixelSize * (l % grid.X), (float)Math.Floor(l / grid.X) * pixelSize), temp: float.Parse(data[1]));
                    if (created == null) { Logger.Log($"Error creating pixel #{l}"); continue; }
                    canvas.Pixels[l] = created;
                }
                catch (Exception e) { Logger.Log($"Error loading pixel #{l}: {e}"); }
            }
            Logger.Log($"Loaded from slot {saveNumber}");
        }
    }
}
