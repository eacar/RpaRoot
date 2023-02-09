using System;

namespace Rpa.WinService.Exceptions
{
    [Serializable]
    public class ActionException : Exception
    {
        #region Properties

        public bool IsRetryable { get; }
        public int MaxRetryCount { get; }
        public int WaitSecondsWhenFailed { get; }

        #endregion

        #region Constructors

        public ActionException(string message = "", bool isRetryable = false, int maxRetryCount = 3, int waitSecondsWhenFailed = 5)
            : base(message)
        {
            IsRetryable = isRetryable;
            MaxRetryCount = maxRetryCount;
            WaitSecondsWhenFailed = waitSecondsWhenFailed;
        }
        public ActionException(string message, Exception innerException, bool isRetryable = false, int maxRetryCount = 3, int waitSecondsWhenFailed = 5)
            : base(message, innerException)
        {
            IsRetryable = isRetryable;
            MaxRetryCount = maxRetryCount;
            WaitSecondsWhenFailed = waitSecondsWhenFailed;
        }

        #endregion
    }
}