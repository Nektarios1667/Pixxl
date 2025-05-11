using System;
using System.Collections.Generic;
using Pixxl.Materials;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl
{
    public static class AirPool
    {
        private static Stack<Materials.Air> pool = new();
        public static Air Get(Xna.Vector2 location, Canvas canvas)
        {
            if (pool.Count > 0)
            {
                Air pixel = pool.Pop();
                pixel.Reset(location);
                return pixel;
            } else
            {   
                return new(location, canvas);
            }
        }
        public static void Return(Air pixel)
        {
            pool.Push(pixel);
        }
    }
}
