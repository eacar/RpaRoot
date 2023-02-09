using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rpa.Extensions
{
    public static class StringExtensions
    {
        #region Fields

        private static readonly Regex DigitRegex = new Regex(@"^\d+$");

        #endregion

        #region Methods - Public

        public static string WildCardToRegular(this string value)
        {
            return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
        }
        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength, char delimiter)
        {
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i)).Replace(delimiter.ToString(), "");
        }
        public static bool IsDigit(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) && DigitRegex.IsMatch(value);
        }
        public static bool IsDouble(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Replace(",", "").Replace(".", "").IsDigit();
            //return !string.IsNullOrWhiteSpace(value) && DoubleRegex.IsMatch(value);
        }
        public static string ExtractOnlyDigits(this string value)
        {
            return string.Join("", value.Where(Char.IsDigit));
        }
        public static string ExtractOnlyLetters(this string value)
        {
            return string.Join("", value.Where(c => !Char.IsDigit(c)));
        }

        public static Tuple<bool, double> GetSimilarity(this string str1, string str2, double minPercentage, bool ignoreCase = true, CultureInfo ci = null)
        {
            if (ignoreCase)
            {
                ci = ci ?? CultureInfo.GetCultureInfo("tr");
                str1 = str1.ToLower(ci);
                str2 = str2.ToLower(ci);
            }

            var totalLength = str1.Length + str2.Length;
            var percentage = 100 - ((GetLevenshteinDistance(str1, str2) * (double)100) / totalLength);

            return new Tuple<bool, double>(percentage >= minPercentage, percentage);
        }
        
        public static string ToClearedText(this string value)
        {
            value = Regex.Replace(value, @"\t|\n|\r", "");
            value = Regex.Replace(value, "[ ]{2,}", " ");
            return value
                .Trim()
                .Replace("&amp;", "")
                .Replace("nbsp;", "");
        }

        public static string ToClearedTextAsLowered(this string value, CultureInfo ci = null)
        {
            if(ci == null) ci = CultureInfo.GetCultureInfo("tr");
            value = ToClearedText(value);
            return value.ToLower(ci);
        }
        public static string ToVarcharSqlValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? $"'{value.Replace("'", "''")}'" : "''";
        }
        public static string ToNullableVarcharSqlValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? $"'{value.Replace("'", "''")}'" : "NULL";
        }
        public static string ToNullableIntSqlValue(this int? value, string overrideValue = "")
        {
            return value?.ToString() ?? (!string.IsNullOrWhiteSpace(overrideValue) ? overrideValue : "NULL");
        }
        public static string ToNullableBooleanSqlValue(this bool? value)
        {
            if (!value.HasValue)
                return "NULL";

            return value.Value.ToBooleanSqlValue();
        }
        public static string ToBooleanSqlValue(this bool value)
        {
            return value ? "1" : "0";
        }
        public static string ToFloatSqlValue(this double value)
        {
            return value.Convert<string>().Replace(",", ".");
        }
        public static string ToNullableFloatSqlValue(this double? value)
        {
            return value.HasValue ? value.Convert<string>().Replace(",", ".") : "NULL";
        }
        public static string ToShorter(this string value, int maxLength = 30, string valueWhenExceeds = "...")
        {
            var result = value.Substring(0,
                maxLength >= value.Length || maxLength < 0
                ? value.Length
                : maxLength);
            return result.Trim() + (maxLength < value.Length ? valueWhenExceeds : string.Empty);
        }

        #endregion

        #region Methods - Private

        private static int GetLevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

        #endregion
    }
}