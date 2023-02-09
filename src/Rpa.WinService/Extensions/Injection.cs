using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Rpa.Extensions;
using Rpa.Log.Extensions;
using Rpa.WinService.Contracts;
using Rpa.WinService.Jobbers;

namespace Rpa.WinService.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddRpaWinServiceServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IJobHandler, JobHandler>();
            services.TryAddSingleton<IJobFactory, JobFactory>();
            services.TryAddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.TryAddSingleton<QuartzJobRunner>();
            services.AddHostedService<JobService>();

            services.AddRpaLogServices();
            services.AddRpaServices();

            return services;
        }
    }
}