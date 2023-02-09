using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rpa.WinService.Extensions
{
    public static class TaskExtensions
    {
        public static Task ParallelForEachAsync<T>(
            this IEnumerable<T> source,
            Func<T, Task> body,
            int degreeOfParallelization = 10)
        {
            async Task AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        await body(partition.Current);
                    }
                }
            }

            return Task.WhenAll(
                Partitioner
                    .Create(source)
                    .GetPartitions(degreeOfParallelization)
                    .AsParallel()
                    .Select(AwaitPartition)
            );
        }

        /// <summary>
        /// Concurrently Executes async actions for each item of <see cref="IEnumerable<typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type of IEnumerable</typeparam>
        /// <param name="enumerable">instance of <see cref="IEnumerable<typeparamref name="T"/>"/></param>
        /// <param name="action">an async <see cref="Action" /> to execute</param>
        /// <param name="maxDegreeOfParallelism">Optional, An integer that represents the maximum degree of parallelism,
        /// Must be grater than 0</param>
        /// <returns>A Task representing an async operation</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the maxActionsToRunInParallel is less than 1</exception>
        public static async Task ForEachAsyncConcurrent<T>(
            this IEnumerable<T> enumerable,
            Func<T, Task> action,
            int? maxDegreeOfParallelism = null)
        {
            if (maxDegreeOfParallelism.HasValue)
            {
                using (var semaphoreSlim = new SemaphoreSlim(
                    maxDegreeOfParallelism.Value, maxDegreeOfParallelism.Value))
                {
                    var tasksWithThrottler = new List<Task>();

                    foreach (var item in enumerable)
                    {
                        // Increment the number of currently running tasks and wait if they are more than limit.
                        await semaphoreSlim.WaitAsync();

                        tasksWithThrottler.Add(Task.Run(async () =>
                        {
                            try
                            {
                                await action(item);
                            }
                            finally
                            {
                                // action is completed, so decrement the number of currently running tasks
                                semaphoreSlim.Release();
                            }

                        }));
                    }

                    // Wait for all tasks to complete.
                    await Task.WhenAll(tasksWithThrottler.ToArray());
                }
            }
            else
            {
                await Task.WhenAll(enumerable.Select(item => action(item)));
            }
        }
    }
}