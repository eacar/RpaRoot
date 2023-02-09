using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rpa.WinService.Enums;

namespace Rpa.WinService.Contracts
{
    public interface IJobHandler
    {
        #region Methods

        #region RunAsync Without Item

        Task RunAsync(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<Exception, Task> actionOnFailure = null,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default);

        Task RunAsync<TException1>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception;

        Task RunAsync<TException1, TException2>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<TException2, Task> actionOnFailure2,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception;

        Task RunAsync<TException1, TException2, TException3>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<TException2, Task> actionOnFailure2,
            Func<TException3, Task> actionOnFailure3,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception;

        Task RunAsync<TException1, TException2, TException3, TException4>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<TException2, Task> actionOnFailure2,
            Func<TException3, Task> actionOnFailure3,
            Func<TException4, Task> actionOnFailure4,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception;

        Task RunAsync<TException1, TException2, TException3, TException4, TException5>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<TException2, Task> actionOnFailure2,
            Func<TException3, Task> actionOnFailure3,
            Func<TException4, Task> actionOnFailure4,
            Func<TException5, Task> actionOnFailure5,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception;

        #endregion

        #region RunAsync With Item

        Task RunAsync<TItem>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<Exception, TItem, Task> actionOnFailure = null,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TItem : IJobItem;

        Task RunAsync<TItem, TException1>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TException1 : Exception
            where TItem : IJobItem;

        Task RunAsync<TItem, TException1, TException2>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1,
            Func<TException2, TItem, Task> actionOnFailure2,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TException1 : Exception
            where TException2 : Exception
            where TItem : IJobItem;

        Task RunAsync<TItem, TException1, TException2, TException3>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1,
            Func<TException2, TItem, Task> actionOnFailure2,
            Func<TException3, TItem, Task> actionOnFailure3,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TItem : IJobItem;

        Task RunAsync<TItem, TException1, TException2, TException3, TException4>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1,
            Func<TException2, TItem, Task> actionOnFailure2,
            Func<TException3, TItem, Task> actionOnFailure3,
            Func<TException4, TItem, Task> actionOnFailure4,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TItem : IJobItem;

        Task RunAsync<TItem, TException1, TException2, TException3, TException4, TException5>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1,
            Func<TException2, TItem, Task> actionOnFailure2,
            Func<TException3, TItem, Task> actionOnFailure3,
            Func<TException4, TItem, Task> actionOnFailure4,
            Func<TException5, TItem, Task> actionOnFailure5,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception
            where TItem : IJobItem;

        #endregion

        #endregion
    }
}
