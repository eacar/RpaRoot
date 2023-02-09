using Rpa.Contracts;
using Rpa.Log.Loggers;
using Rpa.WinService.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rpa.WinService.Jobbers
{
    public class TaskTimerHandler : ITaskTimer, IDisposable
    {
        #region Fields

        private volatile bool _isStarted;
        private CancellationTokenSource _cancellationTokenSource;
        private Func<Task> _timerElapsedListener;
        private readonly IDateTimeHandler _dateTime;

        #endregion

        #region Constructors

        public TaskTimerHandler(IDateTimeHandler dateTime)
        {
            _dateTime = dateTime;
        }

        #endregion

        #region Methods - Public - ITimer

        public void Start(TimeSpan timerPeriod)
        {
            if (_timerElapsedListener == null)
            {
                throw new Exception("Timer elapsed listener is not set");
            }

            if (_isStarted) return;

            _isStarted = true;
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var expectedNextTrigger = _dateTime.Now.Add(timerPeriod);
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {

                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Dispose();
                        return;
                    }

                    try
                    {
                        await _timerElapsedListener();
                    }
                    catch (Exception ex)
                    {
                        SharpLogger.LogError(ex, "An unhandled exception occurred!");
                    }

                    var delay = expectedNextTrigger - _dateTime.Now;
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay);
                    }

                    expectedNextTrigger = _dateTime.Now.Add(timerPeriod);
                }
            }, _cancellationTokenSource.Token);
        }
        public void Stop()
        {
            _isStarted = false;

            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
        }
        public void SetTimerElapsedListener(Func<Task> timerElapsedListener)
        {
            _timerElapsedListener = timerElapsedListener;
        }

        #endregion

        #region Methods - IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }
        ~TaskTimerHandler()
        {
            Dispose(false);
        }

        #endregion
    }
}