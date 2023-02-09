using Rpa.Contracts;
using System;

namespace Rpa.Handlers
{
    public sealed class DateTimeHandler : IDateTimeHandler
    {
        #region Properties - IDateTime

        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => DateTime.Now;

        #endregion

        #region Methods - Public - IDateTime

        public DateTime GetStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }
        public DateTime GetEndOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }
        public bool GetIsToday(DateTime givenDate, DateTime baseDate)
        {
            return GetStartOfDay(givenDate).Equals(GetStartOfDay(baseDate));
        }
        public DateTime ConvertToTimeZonedDate(DateTime utcDate, TimeSpan dateTimeOffset)
        {
            //IMPORTANT!!!!! -> -defaultTimeSpan.Hours is because the times are retrieved to Turkey Standard Time zone! Not UTC
            //We are converting it to UTC and then add the datetimeOffset

            //var defaultTimeSpan = TimeZoneInfo.GetSystemTimeZones().First(c => c.Id == Formats.TurkishTimeZone).BaseUtcOffset;
            //return utcDate.Add(-defaultTimeSpan).Add(dateTimeOffset);

            return utcDate.Add(dateTimeOffset);
        }
        public DateTime ConvertToUtcDate(DateTime givenDate, TimeSpan dateTimeOffset)
        {
            return givenDate.Add(-dateTimeOffset);
        }

        #endregion
    }
}