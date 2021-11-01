using System;

namespace Banks.Extensions
{
    public static class Extensions
    {
        public static bool IsInRange(this Range range, double number)
            => number >= range.Start.Value && number < range.End.Value;
    }
}