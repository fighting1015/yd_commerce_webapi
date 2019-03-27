using Abp.BackgroundJobs;
using Hangfire;
using HangfireRecurringJob = Hangfire.RecurringJob;

namespace Vapps.Web.Hangfire
{
    public static class HangfireBackgroundJobManagerExtension
    {
        /// <summary>
        /// 每隔 X 日执行
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="interval"></param>
        /// <param name="queue"></param>
        public static void DailyInterval<TJob, TArgs>(TArgs args, int interval, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.DayInterval(interval), null, queue);
        }

        /// <summary>
        /// 每日 X时X分执行
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="hourOfUtc"></param>
        /// <param name="minute"></param>
        /// <param name="queue"></param>
        public static void DailyRecurring<TJob, TArgs>(TArgs args, int hourOfUtc, int minute, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Daily(hourOfUtc, minute), null, queue);
        }

        /// <summary>
        /// 每隔 X 小时执行
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="hourInterval"></param>
        /// <param name="queue"></param>
        public static void HourInterval<TJob, TArgs>(TArgs args, int hourInterval, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.HourInterval(hourInterval), null, queue);
        }

        /// <summary>
        /// 每小时执行
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="queue"></param>
        public static void HourRecurring<TJob, TArgs>(TArgs args, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Hourly(), null, queue);
        }

        /// <summary>
        /// 每分钟中心
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="queue"></param>
        public static void MinutelyRecurring<TJob, TArgs>(TArgs args, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.Minutely(), null, queue);
        }

        /// <summary>
        /// 隔 X 分钟执行
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="minuteInterval"></param>
        /// <param name="queue"></param>
        public static void MinuteInterval<TJob, TArgs>(TArgs args, int minuteInterval, string queue = "default") where TJob : IBackgroundJob<TArgs>
        {
            HangfireRecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), Cron.MinuteInterval(minuteInterval), null, queue);
        }
    }
}
