using Rpa.Extensions;
using Rpa.Log.Loggers;
using Rpa.WinService.Contracts;
using Rpa.WinService.Enums;
using Rpa.WinService.Exceptions;
using Rpa.WinService.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rpa.WinService.Jobbers
{
    public sealed class JobHandler : IJobHandler
    {
        #region Fields

        private const double Timeout = 300000;
        private readonly Func<Exception, Task> _noException = null;

        #endregion

        #region Methods - Public - IJobHelper

        #region RunAsync Without Item

        public async Task RunAsync(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<Exception, Task> actionOnFailure = null,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                actionProcess,
                actionOnFailure,
                _noException,
                _noException,
                _noException,
                _noException,
                actionUnhandledFailure,
                timeout);
        }

        public async Task RunAsync<TException1>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                actionProcess,
                actionOnFailure1,
                _noException,
                _noException,
                _noException,
                _noException,
                actionUnhandledFailure,
                timeout);
        }

        public async Task RunAsync<TException1, TException2>(
            JobRunType jobRunType,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1,
            Func<TException2, Task> actionOnFailure2,
            Func<Exception, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                actionProcess,
                actionOnFailure1,
                actionOnFailure2,
                _noException,
                _noException,
                _noException,
                actionUnhandledFailure,
                timeout);
        }

        public async Task RunAsync<TException1, TException2, TException3>(
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
            where TException3 : Exception
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                actionProcess,
                actionOnFailure1,
                actionOnFailure2,
                actionOnFailure3,
                _noException,
                _noException,
                actionUnhandledFailure,
                timeout);
        }

        public async Task RunAsync<TException1, TException2, TException3, TException4>(
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
            where TException4 : Exception
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                actionProcess,
                actionOnFailure1,
                actionOnFailure2,
                actionOnFailure3,
                actionOnFailure4,
                _noException,
                actionUnhandledFailure,
                timeout);
        }

        public async Task RunAsync<TException1, TException2, TException3, TException4, TException5>(
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
            where TException5 : Exception
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                actionProcess,
                actionOnFailure1,
                actionOnFailure2,
                actionOnFailure3,
                actionOnFailure4,
                actionOnFailure5,
                actionUnhandledFailure,
                timeout);
        }

        #endregion

        #region RunAsync With Item

        public async Task RunAsync<TItem>(
            JobRunType jobRunType,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<Exception, TItem, Task> actionOnFailure = null,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            int? sleepSecondsWhenNoData = null,
            int? maxParallelTaskCount = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TItem : IJobItem
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                maxParallelTaskCount,
                actionGetData,
                actionProcessData,
                actionOnFailure,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                actionUnhandledFailure,
                timeoutGetItems,
                timeoutProcessItem
                );
        }

        public async Task RunAsync<TItem, TException1>(
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
            where TItem : IJobItem
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                maxParallelTaskCount,
                actionGetData,
                actionProcessData,
                actionOnFailure1,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                actionUnhandledFailure,
                timeoutGetItems,
                timeoutProcessItem);
        }

        public async Task RunAsync<TItem, TException1, TException2>(
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
            where TItem : IJobItem
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                maxParallelTaskCount,
                actionGetData,
                actionProcessData,
                actionOnFailure1,
                actionOnFailure2,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                actionUnhandledFailure,
                timeoutGetItems,
                timeoutProcessItem);
        }

        public async Task RunAsync<TItem, TException1, TException2, TException3>(
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
            where TItem : IJobItem
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                maxParallelTaskCount,
                actionGetData,
                actionProcessData,
                actionOnFailure1,
                actionOnFailure2,
                actionOnFailure3,
                (Func<Exception, TItem, Task>)null,
                (Func<Exception, TItem, Task>)null,
                actionUnhandledFailure,
                timeoutGetItems,
                timeoutProcessItem);
        }

        public async Task RunAsync<TItem, TException1, TException2, TException3, TException4>(
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
            where TItem : IJobItem
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                maxParallelTaskCount,
                actionGetData,
                actionProcessData,
                actionOnFailure1,
                actionOnFailure2,
                actionOnFailure3,
                actionOnFailure4,
                (Func<Exception, TItem, Task>)null,
                actionUnhandledFailure,
                timeoutGetItems,
                timeoutProcessItem);
        }

        public async Task RunAsync<TItem, TException1, TException2, TException3, TException4, TException5>(
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
            where TItem : IJobItem
        {
            await RunCompleteAsync(
                jobRunType,
                sleepSecondsWhenNoData,
                maxParallelTaskCount,
                actionGetData,
                actionProcessData,
                actionOnFailure1,
                actionOnFailure2,
                actionOnFailure3,
                actionOnFailure4,
                actionOnFailure5,
                actionUnhandledFailure,
                timeoutGetItems,
                timeoutProcessItem);
        }

        #endregion

        #endregion

        #region Methods - Private - Helper Run

        private async Task RunCompleteAsync<TException1, TException2, TException3, TException4, TException5>(
            JobRunType jobRunType,
            int? sleepWhenNoData,
            Func<Task> actionProcess,
            Func<TException1, Task> actionOnFailure1 = null,
            Func<TException2, Task> actionOnFailure2 = null,
            Func<TException3, Task> actionOnFailure3 = null,
            Func<TException4, Task> actionOnFailure4 = null,
            Func<TException5, Task> actionOnFailure5 = null,
            Func<Exception, Task> actionUnhandledFailure = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception
        {
            var funcName = actionProcess.Target.GetType().FullName + ".ProcessItem";
            var sleepWhenNoDataSeconds = sleepWhenNoData ?? 10;
            
            Func<Task> actionRecursive = async () =>
            {
                await actionProcess.Invoke();
            };

            switch (jobRunType)
            {
                case JobRunType.OneTime:
                case JobRunType.Scheduled:

                    //using (Operation.Time(actionProcessDataName))
                    //{
                    try
                    {
                        await RunTimeoutTask(actionRecursive, timeout);
                    }
                    catch (Exception ex)
                    {
                        switch (ex)
                        {
                            case TException1 exception1 when actionOnFailure1 != null:
                                await CatchExceptionAsync(exception1, funcName, actionOnFailure1, actionRecursive);
                                break;
                            case TException2 exception2 when actionOnFailure2 != null:
                                await CatchExceptionAsync(exception2, funcName, actionOnFailure2, actionRecursive);
                                break;
                            case TException3 exception3 when actionOnFailure3 != null:
                                await CatchExceptionAsync(exception3, funcName, actionOnFailure3, actionRecursive);
                                break;
                            case TException4 exception4 when actionOnFailure4 != null:
                                await CatchExceptionAsync(exception4, funcName, actionOnFailure4, actionRecursive);
                                break;
                            case TException5 exception5 when actionOnFailure5 != null:
                                await CatchExceptionAsync(exception5, funcName, actionOnFailure5, actionRecursive);
                                break;
                            case TimeoutException timeoutException:
                                SharpLogger.LogError($"{funcName} | {timeoutException.Message}");
                                break;
                            default:
                                {
                                    if (actionUnhandledFailure != null)
                                        await actionUnhandledFailure.Invoke(ex);
                                    else
                                        SharpLogger.LogError($"{funcName} | {ex}"); //Just log and continue. It will retry next time.

                                    break;
                                }
                        }
                    }
                    //}

                    break;
                case JobRunType.Always:
                    while (true)
                    {
                        //using (Operation.Time($"{actionProcessDataName}"))
                        //{
                        try
                        {
                            await RunTimeoutTask(actionProcess, timeout);
                        }
                        catch (Exception ex)
                        {
                            switch (ex)
                            {
                                case TException1 exception1 when actionOnFailure1 != null:
                                    await CatchExceptionAsync(exception1, funcName, actionOnFailure1, actionRecursive);
                                    break;
                                case TException2 exception2 when actionOnFailure2 != null:
                                    await CatchExceptionAsync(exception2, funcName, actionOnFailure2, actionRecursive);
                                    break;
                                case TException3 exception3 when actionOnFailure3 != null:
                                    await CatchExceptionAsync(exception3, funcName, actionOnFailure3, actionRecursive);
                                    break;
                                case TException4 exception4 when actionOnFailure4 != null:
                                    await CatchExceptionAsync(exception4, funcName, actionOnFailure4, actionRecursive);
                                    break;
                                case TException5 exception5 when actionOnFailure5 != null:
                                    await CatchExceptionAsync(exception5, funcName, actionOnFailure5, actionRecursive);
                                    break;
                                case TimeoutException timeoutException:
                                    SharpLogger.LogError($"{funcName} | {timeoutException.Message}");
                                    break;
                                default:
                                    {
                                        if (actionUnhandledFailure != null) await actionUnhandledFailure.Invoke(ex);
                                        else
                                        {
                                            SharpLogger.LogError($"{funcName} | {ex}"); //Just log and continue. It will retry next time.
                                        }

                                        break;
                                    }
                            }
                        }
                        //}

                        //SharpLogger.LogInfo($"Now, we wait for {TimeSpan.FromSeconds(sleepWhenNoDataSeconds)}");
                        Thread.Sleep(TimeSpan.FromSeconds(sleepWhenNoDataSeconds));
                    }
            }
        }

        private async Task RunCompleteAsync<TItem, TException1, TException2, TException3, TException4, TException5>(
            JobRunType jobRunType,
            int? sleepWhenNoData,
            int? maxParallelTaskCount,
            Func<Task<IEnumerable<TItem>>> actionGetData,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1 = null,
            Func<TException2, TItem, Task> actionOnFailure2 = null,
            Func<TException3, TItem, Task> actionOnFailure3 = null,
            Func<TException4, TItem, Task> actionOnFailure4 = null,
            Func<TException5, TItem, Task> actionOnFailure5 = null,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            double timeoutGetItems = default,
            double timeoutProcessItem = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception
            where TItem : IJobItem
        {
            var sleepWhenNoDataSeconds = sleepWhenNoData ?? 10;
            IEnumerable<TItem> list;
            switch (jobRunType)
            {
                case JobRunType.OneTime:
                case JobRunType.Scheduled:

                    list = await RunGetItemsAsync(actionGetData, timeoutGetItems);

                    if (list != null && list.Any())
                    {
                        await RunProcessItemAsync(
                            maxParallelTaskCount,
                            list,
                            actionProcessData,
                            actionOnFailure1,
                            actionOnFailure2,
                            actionOnFailure3,
                            actionOnFailure4,
                            actionOnFailure5,
                            actionUnhandledFailure,
                            timeoutProcessItem);
                    }

                    break;
                case JobRunType.Always:
                    while (true)
                    {
                        list = await RunGetItemsAsync(actionGetData, timeoutGetItems);

                        if (list != null && list.Any())
                        {
                            await RunProcessItemAsync(
                                maxParallelTaskCount,
                                list,
                                actionProcessData,
                                actionOnFailure1,
                                actionOnFailure2,
                                actionOnFailure3,
                                actionOnFailure4,
                                actionOnFailure5,
                                actionUnhandledFailure,
                                timeoutProcessItem);
                        }
                        else
                        {
                            //SharpLogger.LogInfo($"{actionProcessData.Target.GetType().FullName} No data found! So we wait for {TimeSpan.FromSeconds(sleepWhenNoDataSeconds)}");
                            Thread.Sleep(TimeSpan.FromSeconds(sleepWhenNoDataSeconds));
                        }
                    }
            }
        }

        private async Task<IEnumerable<TItem>>  RunGetItemsAsync<TItem>
        (
            Func<Task<IEnumerable<TItem>>> actionGetData,
            double timeout = default)
            where TItem : IJobItem
        {
            IEnumerable<TItem> result = new List<TItem>();

            var funcName = actionGetData.Target.GetType().FullName + ".GetItem";

            Func<Task> actionRecursive = async () =>
            {
                //using (Operation.Time(actionProcessDataName))
                //{

                var rsp = await actionGetData.Invoke();
                result = rsp?.ToList() ?? new List<TItem>();
                if (result.Any())
                {
                    SharpLogger.LogInfo($"{funcName} found {result.Count()} item!");
                }
                //}
            };

            try
            {
                await RunTimeoutTask(actionRecursive, timeout);
            }
            catch (TimeoutException ex)
            {
                SharpLogger.LogError($"{funcName} | {ex.Message}"); //Just log and continue. It will retry next time.
            }
            catch (Exception ex)
            {
                SharpLogger.LogError($"{funcName} | {ex}"); //Just log and continue. It will retry next time.
            }

            return result;
        }

        private async Task RunProcessItemAsync<TItem, TException1, TException2, TException3, TException4, TException5>(
            int? maxParallelTaskCount,
            IEnumerable<TItem> list,
            Func<TItem, Task> actionProcessData,
            Func<TException1, TItem, Task> actionOnFailure1,
            Func<TException2, TItem, Task> actionOnFailure2,
            Func<TException3, TItem, Task> actionOnFailure3,
            Func<TException4, TItem, Task> actionOnFailure4,
            Func<TException5, TItem, Task> actionOnFailure5,
            Func<Exception, TItem, Task> actionUnhandledFailure = null,
            double timeout = default)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            where TException4 : Exception
            where TException5 : Exception
            where TItem : IJobItem
        {
            var funcName = actionProcessData.Target.GetType().FullName + ".ProcessItem";

            //SharpLogger.LogInfo($"{actionProcessDataName} will process {list.Count()} items...");

            //using (Operation.Time($"{actionProcessDataName}"))
            //{

            await list.ParallelForEachAsync(async c =>
            {
                {
                    Func<Task> actionRecursive = async () =>
                    {
                        await actionProcessData.Invoke(c);
                    };

                    try
                    {
                        await RunTimeoutTask(actionRecursive, timeout);
                    }
                    catch (Exception ex)
                    {
                        switch (ex)
                        {
                            case TException1 exception1 when actionOnFailure1 != null:
                                await CatchExceptionAsync(exception1, funcName, actionOnFailure1, actionRecursive, c);
                                break;
                            case TException2 exception2 when actionOnFailure2 != null:
                                await CatchExceptionAsync(exception2, funcName, actionOnFailure2, actionRecursive, c);
                                break;
                            case TException3 exception3 when actionOnFailure3 != null:
                                await CatchExceptionAsync(exception3, funcName, actionOnFailure3, actionRecursive, c);
                                break;
                            case TException4 exception4 when actionOnFailure4 != null:
                                await CatchExceptionAsync(exception4, funcName, actionOnFailure4, actionRecursive, c);
                                break;
                            case TException5 exception5 when actionOnFailure5 != null:
                                await CatchExceptionAsync(exception5, funcName, actionOnFailure5, actionRecursive, c);
                                break;
                            case TimeoutException timeoutException:
                                SharpLogger.LogError($"{funcName} | {timeoutException.Message}");
                                break;
                            default:
                                {
                                    if (actionUnhandledFailure != null)
                                    {
                                        await actionUnhandledFailure.Invoke(ex, c);
                                    }
                                    else
                                    {
                                        SharpLogger.LogError($"{funcName} | {ex}", "[NOTE: There is an unhandled! Skipping the item...]");
                                        //This one must stay here because when we call this as a invoke function it does not log the exception in the invoking part
                                    }

                                    break;
                                }
                        }
                    }
                }
            }, maxParallelTaskCount ?? 10);
        }

        private async Task CatchExceptionAsync<TException>(
            TException ex,
            string processName,
            Func<TException, Task> actionOnFailure,
            Func<Task> actionRecursive,
            int retryCount = 1)
            where TException : Exception
        {
            try
            {
                if (ex is ActionException actionException)
                {
                    if (actionException.IsRetryable && retryCount <= actionException.MaxRetryCount)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(actionException.WaitSecondsWhenFailed));
                        SharpLogger.LogWarn($"{processName} is now retying... RetryCount: {retryCount} MaxRetryCount: {actionException.MaxRetryCount} Had Exception: {actionException}");

                        await actionRecursive.Invoke();
                    }
                    else
                    {
                        await HandleActionFailure(actionOnFailure, ex, processName);
                    }
                }
                else
                {
                    await HandleActionFailure(actionOnFailure, ex, processName);
                }
            }
            catch (Exception exMain)
            {
                SharpLogger.LogError($"{processName} | {exMain}", "[NOTE: There is an unhandled exception within Catch Exception!]");
                await CatchExceptionAsync(ex, processName, actionOnFailure, actionRecursive, ++retryCount);
                //Do not throw any further!
            }
        }

        private async Task CatchExceptionAsync<TException, TItem>(
            TException ex,
            string processName,
            Func<TException, TItem, Task> actionOnFailure,
            Func<Task> actionRecursive,
            TItem item = default,
            int retryCount = 1)
            where TException : Exception
            where TItem : IJobItem
        {
            try
            {
                if (ex is ActionException actionException)
                {
                    if (actionException.IsRetryable && retryCount <= actionException.MaxRetryCount)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(actionException.WaitSecondsWhenFailed));
                        SharpLogger.LogWarn($"{processName} is now retying... RetryCount: {retryCount} MaxRetryCount: {actionException.MaxRetryCount} Had Exception: {actionException}");

                        await actionRecursive.Invoke();
                    }
                    else
                    {
                        await HandleActionFailure(actionOnFailure, ex, item, processName);
                    }
                }
                else
                {
                    await HandleActionFailure(actionOnFailure, ex, item, processName);
                }
            }
            catch (Exception exMain)
            {
                SharpLogger.LogError($"{processName} | {exMain}", "[NOTE: There is an unhandled exception within Catch Exception!]");
                await CatchExceptionAsync(ex, processName, actionOnFailure, actionRecursive, item, ++retryCount);
                //Do not throw any further!
            }
        }

        private async Task HandleActionFailure<TException>(Func<TException, Task> actionOnFailure, TException ex, string processName)
            where TException : Exception
        {
            if (actionOnFailure != null)
            {
                try
                {
                    //Note: This logging is disabled because since the code below will be invoked ONLY developer has an override action. Therefore, we are letting
                    //the developer to let it decide
                    //SharpLogger.LogError(ex, $"{processName} failed!");
                    await actionOnFailure.Invoke(ex);
                }
                catch (Exception exFailure)
                {
                    SharpLogger.LogError($"{processName} | {exFailure}", $"Failed! [NOTE: The action on failure is also failed!]");
                    //Do not throw any further!
                }
            }
            else
            {
                SharpLogger.LogError($"{processName} | {ex}", $"Failed! [NOTE: There is handler specified! Skipping the item...]");
            }
        }
        private async Task HandleActionFailure<TException, TItem>(Func<TException, TItem, Task> actionOnFailure, TException ex, TItem item, string processName)
            where TException : Exception
            where TItem : IJobItem
        {
            if (actionOnFailure != null)
            {
                try
                {
                    //Note: This logging is disabled because since the code below will be invoked ONLY developer has an override action. Therefore, we are letting
                    //the developer to let it decide
                    //SharpLogger.LogError(ex, $"{processName} failed!");
                    await actionOnFailure.Invoke(ex, item);
                }
                catch (Exception exFailure)
                {
                    SharpLogger.LogError($"{processName} | {exFailure}", $"Failed! [NOTE: The action on failure is also failed!]");
                    //Do not throw any further!
                }
            }
            else
            {
                SharpLogger.LogError($"{processName} | {ex}", $"Failed! [NOTE: There is handler specified! Skipping the item...]");
            }
        }

        private async Task RunTimeoutTask(Func<Task> funcTask, double timeout)
        {
            if (timeout <= 0)
                timeout = Timeout;

            await funcTask.Invoke().WaitAsync(TimeSpan.FromMilliseconds(timeout));
        }

        #endregion
    }
}