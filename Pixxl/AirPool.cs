using Microsoft.CodeAnalysis;
using Pixxl.Materials;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pixxl
{
    public static class AirPool
    {
        private static readonly Stack<Air> pool = new();
        public static Air Get(Vector2 location, Canvas canvas)
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
        // TODO Sometimes returned pixels can update after being returned
        public static void Return(Air pixel)
        {
            return;
            pool.Push(pixel);
        }
    }
}
