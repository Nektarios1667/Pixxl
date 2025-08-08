using Microsoft.Xna.Framework;

namespace Pixxl.Tools;

public static class Functions
{
    public static Color Lighten(Color color, float percentage)
    {
        int r = color.R; int g = color.G; int b = color.B;
        r += (int)((255 - r) * percentage);
        g += (int)((255 - g) * percentage);
        b += (int)((255 - b) * percentage);
        return new(r, g, b);
    }
    public static void NoFunc(params object[] _) { }
}

public static class IntExtensions
{
    public static bool IsEven(this int i) => i % 2 == 0;
    public static bool IsOdd(this int i) => i % 2 == 1;
}