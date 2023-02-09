using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Rpa.Extensions
{
    public static class ConversionExceptions
    {
        #region Methods - Public

        public static T ExtractPrice<T>(this string value, T defaultValue = default(T))
        {
            return ExtractPriceAsString(value).Convert<T>(defaultValue);
        }
        public static string ExtractPriceAsString(this string value)
        {
            value = value.Replace(" ", "");
            var sb = new StringBuilder();
            foreach (char c in value)
            {
                if (Char.IsDigit(c) || c == '.' || c == ',')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
        public static T Convert<T>(this object value, T defaultValue = default(T))
        {
            try
            {
                if (value == null)
                    return defaultValue;

                if (value.GetType().IsEnum)
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)value.ToString();

                    return (T)value;
                }

                if (typeof(T) == typeof(double))
                {
                    return (T)(object)value.ToMoney<double>();
                }
                if (typeof(T) == typeof(decimal))
                {
                    return (T)(object)value.ToMoney<decimal>();
                }
                if (typeof(T) == typeof(float))
                {
                    return (T)(object)value.ToMoney<float>();
                }

                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromInvariantString(value.ToString());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        public static dynamic CastToDynamic(this object obj, Type castTo)
        {
            return System.Convert.ChangeType(obj, castTo);
        }
        public static double ConvertBytesToMb(this long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        public static string ToQueryString(this object obj)
        {
            var result = new List<string>();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                if (value is ICollection enumerable)
                {
                    result.AddRange(from object v in enumerable
                        select
                            $"{p.Name}={HttpUtility.UrlEncode(v.ToString())}");
                }
                else
                {
                    result.Add($"{p.Name}={HttpUtility.UrlEncode(value.ToString())}");
                }
            }

            return string.Join("&", result.ToArray());
        }

        #endregion

        #region Methods - Private

        private static T ToMoney<T>(this object value)
        {
            value = value.ToString().Replace(" ", "");
            if (value.ToString().IndexOf(".", StringComparison.Ordinal) >= 0 && value.ToString().IndexOf(",", StringComparison.Ordinal) >= 0)
            {
                if (value.ToString().IndexOf(".", StringComparison.Ordinal) < value.ToString().IndexOf(",", StringComparison.Ordinal))
                {
                    value = value.ToString().Replace(".", "");
                    return ParseMoney<T>(value.ToString(), CultureInfo.GetCultureInfo("tr"));
                }

                value = value.ToString().Replace(",", "");
                return ParseMoney<T>(value.ToString(), CultureInfo.GetCultureInfo("en"));
            }

            var dicChar = GetCharConvertDictionary(value.ToString());

            if (dicChar.Count == 1 && dicChar.Values.First() == 1)
            {
                value = GetCharDictionaryValue(dicChar, value.ToString());

                if (value.ToString().Length - value.ToString().LastIndexOf(dicChar.Keys.First()) == 4
                    && value.ToString().Length <= 7) //If it is something like 6.232 we accept this a full value
                {
                    return ParseMoney<T>(value.ToString().Replace(dicChar.Keys.First().ToString(), ""), CultureInfo.InvariantCulture);
                }

                return ParseMoney<T>(value.ToString(), dicChar.Keys.First() == '.'
                    ? CultureInfo.GetCultureInfo("en")
                    : CultureInfo.GetCultureInfo("tr"));
            }

            if (dicChar.Count == 1 && dicChar.Values.First() > 1)
            {
                if (value.ToString().Length - value.ToString().LastIndexOf(dicChar.Keys.First()) == 3) //If it is something like 38.400.00 we accept this a full value
                {
                    value = GetCharDictionaryValue(dicChar, value.ToString());

                    return ParseMoney<T>(value.ToString(), dicChar.Keys.First() == '.'
                        ? CultureInfo.GetCultureInfo("en")
                        : CultureInfo.GetCultureInfo("tr"));
                }

                value = Regex.Replace(value.ToString(), @"[^\d]", "");
            }

            return ParseMoney<T>(value.ToString(), CultureInfo.InvariantCulture);
        }
        private static T ParseMoney<T>(string value, CultureInfo ci, NumberStyles numberStyles = NumberStyles.Currency)
        {
            switch (typeof(T))
            {
                case Type type when type == typeof(float):
                    return (T)(object)float.Parse(value, numberStyles, ci);

                case Type type when type == typeof(double):
                    return (T)(object)double.Parse(value, numberStyles, ci);

                case Type type when type == typeof(decimal):
                    return (T)(object)decimal.Parse(value, numberStyles, ci);
            }

            return default(T);
        }
        private static Dictionary<char, int> GetCharConvertDictionary(string value)
        {
            var dicChar = new Dictionary<char, int>();
            foreach (var c in value)
            {
                if (!Char.IsDigit(c) && c != '-')
                {
                    if (dicChar.ContainsKey(c))
                    {
                        dicChar[c]++;
                    }
                    else
                    {
                        dicChar.Add(c, 1);
                    }
                }
            }

            return dicChar;
        }

        private static string GetCharDictionaryValue(Dictionary<char, int> dicChar, string value)
        {
            var c = dicChar.Keys.First();
            var vals = value.Split(c);
            var newVal = "";
            for (int i = 0; i < vals.Length; i++)
            {
                if (i < vals.Length - 1)
                    newVal += vals[i];
                else
                {
                    newVal += c + vals[i];
                }
            }

            return newVal;
        }

        #endregion
    }
}