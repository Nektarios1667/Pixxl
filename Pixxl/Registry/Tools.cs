using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = { "Clear" };
        public static readonly Color[] Colors = { Color.DarkRed };
        public static readonly Delegate[] Functions = { Reset.All };
    }
}
