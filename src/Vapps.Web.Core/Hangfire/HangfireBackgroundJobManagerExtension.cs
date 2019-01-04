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
        public static void DailyRecurring<TJob, TArgs>(TArgs args, int hourOfUtc, int minute) where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Daily(hourOfUtc, minute));
        }


        public static void MinutelyRecurring<TJob, TArgs>(TArgs args) where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Minutely());
        }
    }
}
