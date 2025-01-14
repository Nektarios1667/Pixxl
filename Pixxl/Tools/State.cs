using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
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
        public static void Save(Canvas canvas)
        {
            // Data
            string data = $"{Constants.Screen.Grid[0]}x{Constants.Screen.Grid[1]}\n{Constants.Screen.PixelSize}\n";
            for (int i = 0; i < canvas.Pixels.Length; i++)
            {
                Pixel current = canvas.Pixels[i];
                data += $"{current.TypeId};{Math.Round(current.Temperature, 2)}\n";
            }
            // Writing
            Logger.Log("Saved Pixel state to 'Saves/save.pxs'");
            File.WriteAllText("Saves/save.pxs", data);
        }
        public static void Load(Canvas canvas)
        {
            // Reading
            string[] lines;
            try
            {
                lines = File.ReadAllLines("Saves/save.pxs");
            }
            catch (FileNotFoundException)
            {
                Logger.Log($"File 'save.pxs' not found"); return;
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
            } catch(IndexOutOfRangeException) { Logger.Log("File header data corrupted or in wrong format"); return; }
            catch(FormatException) { Logger.Log("File header data corrupted or in wrong format"); return; }

            // Loading
            int l = 0;
            foreach (string line in lines[2..])
            {
                try
                {
                    string[] data = line.Split(';');
                    Pixel? created = Canvas.New(canvas, Registry.Materials.Names[int.Parse(data[0])], new(pixelSize * (l % grid.X), (float)Math.Floor(l / grid.X) * pixelSize), temp: float.Parse(data[1]));
                    if (created == null) { Logger.Log($"Error creating pixel #{l}"); continue; }
                    canvas.Pixels[l] = created;
                } catch (Exception e) { Logger.Log($"Error loading pixel #{l}: {e}"); }
                l++;
            }
            Logger.Log("Loaded Pixel state from 'Saves/save.pxs'");
        }
    }
}
