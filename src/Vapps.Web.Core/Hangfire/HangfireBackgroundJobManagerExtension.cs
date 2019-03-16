using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Hangfire;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangfireRecurringJob = Hangfire.RecurringJob;

namespace Vapps.Web.Hangfire
{
    public static class HangfireBackgroundJobManagerExtension
    {
        public static void DailyRecurring<TJob, TArgs>(TArgs args, int hourOfUtc, int minute, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Daily(hourOfUtc, minute), null, queue);
        }

        public static void HourInterval<TJob, TArgs>(TArgs args, int hourInterval, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.HourInterval(hourInterval), null, queue);
        }


        public static void MinutelyRecurring<TJob, TArgs>(TArgs args, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Minutely(), null, queue);
        }

        public static void MinuteInterval<TJob, TArgs>(TArgs args, int minuteInterval, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.MinuteInterval(minuteInterval), null, queue);
        }
    }
}
