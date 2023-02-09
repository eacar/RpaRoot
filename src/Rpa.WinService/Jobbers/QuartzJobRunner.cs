using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Rpa.Log.Loggers;

namespace Rpa.WinService.Jobbers
{
    [DisallowConcurrentExecution]
    public sealed class QuartzJobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzJobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    if (scope.ServiceProvider.GetRequiredService(context.JobDetail.JobType) is IJob job)
                    {
                        //Currently, its an unnecessary log, so we disabled it.
                        //SharpLogger.LogInfo($"Job '{context.JobDetail.JobType}' is now being executed...");
                        await job.Execute(context);
                    }
                }
            }
            catch (Exception ex)
            {
                SharpLogger.LogFatal($"{context.JobDetail.JobType} is now terminated! Ex: {ex}");
                throw;
            }
        }
    }
}