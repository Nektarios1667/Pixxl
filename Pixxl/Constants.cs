using System.Xml.Linq;
using Xna = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixxl;

namespace Pixxl
{
    public static class Const
    {
        public static readonly int RoomTemp = 70;
        public static readonly int[] Window = [1200, 900];
        public static readonly int PixelSize = 5;
        public static readonly float Gravity = 9.81f * PixelSize;
        public static readonly int[] Grid = [Window[0] / PixelSize, Window[1] / PixelSize];
    }
}