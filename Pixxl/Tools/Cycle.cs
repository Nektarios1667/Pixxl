using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixxl.Tools;
public class Cycle
{
    private int _value;
    public int Value {
        get => _value;
        set { _value = value < Min ? Max - 1 : (value >= Max ? Min : value); }
    }
    public int Min { get; set; }
    public int Max { get; set; }
    public Cycle(int value, int min, int max)
    {
        Value = value;
        Min = min;
        Max = max;
    }
    public bool IsEven() => Value % 2 == 0;
    public bool IsOdd() => Value % 2 == 1;
    public static Cycle operator ++(Cycle c) {
        c.Value++;
        return c;
    }
    public static Cycle operator --(Cycle c) {
        c.Value--;
        return c;
    }
}
