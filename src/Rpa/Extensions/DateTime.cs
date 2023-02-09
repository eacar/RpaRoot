using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Rpa.Extensions
{
    public static class DateTimeExtensions
    {
        #region Methods - Public

        public static DateTime ToUnixDateTime(this double unixTimeStamp)
        {
            if (unixTimeStamp <= 0)
                return DateTime.MinValue;

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }

        public static double ToUnixTime(this DateTime date)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return (date.Subtract(dtDateTime)).TotalMilliseconds;
        }
        public static DateTime ToUnixDateTime(this long unixTimeStamp)
        {
            return unixTimeStamp.ToUnixDateTime();
        }
        public static long ToUnixTimeLong(this DateTime date)
        {
            return (long) date.ToUnixTime();
        }

        public static DateTime TryParseDmy(this string value, params string[] alternativeFormats)
        {
            DateTime result = DateTime.MinValue;

            var ci = CultureInfo.CurrentCulture;
            var dateTimeStyle = DateTimeStyles.None;
            var dateTimeFormat = ci.DateTimeFormat.ShortDatePattern + " " + ci.DateTimeFormat.LongTimePattern;
            var formats = new List<string>();

            formats.Add(dateTimeFormat);
            formats.AddRange(alternativeFormats);


            formats.AddRange(alternativeFormats.Select(c => $"{c} HH:mm:ss"));
            formats.AddRange(alternativeFormats.Select(c => $"{c} hh:mm:ss tt"));

            foreach (var format in formats)
            {
                DateTime.TryParseExact(value, format, ci, dateTimeStyle, out result);
                if (result != DateTime.MinValue)
                    break;
            }
            //if (result == DateTime.MinValue)
            //{
            //    throw new FormatException($"Value {value} could not be parsed!");
            //}

            return result;
        }

        public static DateTime ToBeginningOfDay(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }

        public static DateTime ToEndOfDay(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }

        #endregion
    }
}