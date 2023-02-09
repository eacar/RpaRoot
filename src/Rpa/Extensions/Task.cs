using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rpa.Extensions
{
    public static class TaskExtensions
    {
        //public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        //{
        //    using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        //    {
        //        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
        //        if (completedTask == task)
        //        {
        //            timeoutCancellationTokenSource.Cancel();
        //            return await task;  // Very important in order to propagate exceptions
        //        }

        //        throw new TimeoutException("The operation has timed out.");
        //    }
        //}
        public static Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (timeout < TimeSpan.Zero && timeout != TimeSpan.MinValue)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            return task
                .ContinueWith(_ => { }, cts.Token,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
                .ContinueWith(continuation =>
                {
                    cts.Dispose();
                    if (task.IsCompleted)
                        return task;
                    cancellationToken.ThrowIfCancellationRequested();
                    if (continuation.IsCanceled)
                        throw new TimeoutException($"The operation has timed out after {timeout} amount of time.");
                    return task;
                }, TaskScheduler.Default).Unwrap();
        }
        public static Task WaitAsync(this Task task, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (timeout < TimeSpan.Zero && timeout != TimeSpan.MinValue)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            return task
                .ContinueWith(_ => { }, cts.Token,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
                .ContinueWith(continuation =>
                {
                    cts.Dispose();
                    if (task.IsCompleted)
                        return task;
                    cancellationToken.ThrowIfCancellationRequested();
                    if (continuation.IsCanceled)
                        throw new TimeoutException($"The operation has timed out after {timeout} amount of time.");
                    return task;
                }, TaskScheduler.Default).Unwrap();
        }
    }
}