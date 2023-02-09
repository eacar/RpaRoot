using System;
using Rpa.WinService.Enums;

namespace Rpa.WinService.Jobbers
{
    public sealed class JobSchedule
    {
        #region Properties

        public Type JobType { get; }
        public string CronExpression { get; }
        public JobRunType JobRunType { get; }

        #endregion

        #region Constructors

        public JobSchedule(Type jobType, string cronExpression, JobRunType jobRunType)
        {
            JobType = jobType;
            CronExpression = cronExpression;
            JobRunType = jobRunType;
        }

        #endregion
    }
}