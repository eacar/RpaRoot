using System;

namespace Rpa.Contracts
{
    public interface IDateTimeHandler
    {
        #region Properties

        DateTime UtcNow { get; }
        DateTime Now { get; }

        #endregion

        #region Methods 

        DateTime GetStartOfDay(DateTime dateTime);
        DateTime GetEndOfDay(DateTime dateTime);
        bool GetIsToday(DateTime givenDate, DateTime baseDate);
        DateTime ConvertToTimeZonedDate(DateTime utcDate, TimeSpan dateTimeOffset);
        DateTime ConvertToUtcDate(DateTime givenDate, TimeSpan dateTimeOffset);

        #endregion
    }
}