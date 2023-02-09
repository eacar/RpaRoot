using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Logging;
using Quartz.Spi;
using Rpa.Log.Loggers;
using Rpa.WinService.Enums;

namespace Rpa.WinService.Jobbers
{
    public sealed class JobService : IHostedService
    {
        #region Fields

        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobSchedules;
        private IScheduler _scheduler;

        #endregion

        #region Constructors

        public JobService(
            ISchedulerFactory schedulerFactory,
            IEnumerable<JobSchedule> jobSchedules,
            IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        #endregion

        #region Methods - Public - IHostedService

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                LogProvider.IsDisabled = true;
                var properties = new NameValueCollection();

                _scheduler = await SchedulerBuilder.Create(properties)
                    // default max concurrency is 10
                    .UseDefaultThreadPool(x => x.MaxConcurrency = int.MaxValue)
                    .BuildScheduler();
                //_scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                _scheduler.JobFactory = _jobFactory;

                foreach (var jobSchedule in _jobSchedules)
                {
                    var job = CreateJob(jobSchedule);
                    var trigger = CreateTrigger(jobSchedule);

                    await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                }

                await _scheduler.Start(cancellationToken);
            }
            catch (System.Exception ex)
            {
                SharpLogger.LogError(ex, "Job could not be started!");
                throw;
            }
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _scheduler.Shutdown(cancellationToken);
            }
            catch (System.Exception ex)
            {
                SharpLogger.LogError(ex, "Job could not be stopped!");
                throw;
            }
        }

        #endregion

        #region Methods - Private

        private ITrigger CreateTrigger(JobSchedule schedule)
        {
            try
            {
                if (schedule.JobRunType == JobRunType.Scheduled)
                {
                    SharpLogger.LogInfo($"{schedule.JobType.FullName} is triggered with Cron {schedule.CronExpression}");

                    return TriggerBuilder
                        .Create()
                        .WithIdentity($"{schedule.JobType.FullName}.trigger")
                        .WithCronSchedule(schedule.CronExpression)
                        //.WithSimpleSchedule()
                        .WithDescription(schedule.CronExpression)
                        .Build();
                }

                SharpLogger.LogInfo($"{schedule.JobType.FullName} is triggered with {schedule.JobRunType}");

                return TriggerBuilder
                    .Create()
                    .WithIdentity($"{schedule.JobType.FullName}.trigger")
                    //.WithCronSchedule(schedule.CronExpression)
                    .WithSimpleSchedule()
                    .WithDescription(schedule.CronExpression)
                    .Build();
            }
            catch (System.Exception ex)
            {
                SharpLogger.LogError(ex, $"{schedule.JobType} could not be triggered!");
                throw;
            }
        }
        private IJobDetail CreateJob(JobSchedule schedule)
        {
            try
            {
                var jobType = schedule.JobType;
                return JobBuilder
                    .Create(jobType)
                    .WithIdentity(jobType.FullName)
                    .WithDescription(jobType.Name)
                    .Build();
            }
            catch (System.Exception ex)
            {
                SharpLogger.LogError(ex, $"{schedule.JobType} could not be created!");
                throw;
            }
        }

        #endregion
    }
}