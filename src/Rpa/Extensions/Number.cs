using System;
using System.Globalization;

namespace Rpa.Extensions
{
    public static class NumberExtensions
    {
        public static double ToPercentage(this int dataCount, int totalCount, int round = 2)
        {
            return Math.Round((double)dataCount * 100 / totalCount, round);
        }

        public static string ToFormattedNumber(this decimal value, CultureInfo ci = null)
        {
            return value.ToString("N", ci = ci ?? CultureInfo.CurrentCulture);
        }
    }
}