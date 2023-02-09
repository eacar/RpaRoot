using System;
using System.Threading.Tasks;

namespace Rpa.WinService.Contracts
{
    public interface ITaskTimer
    {
        #region Methods

        void Start(TimeSpan timerPeriod);
        void Stop();
        void SetTimerElapsedListener(Func<Task> timerElapsedListener);

        #endregion
    }
}