using Quartz;
using Quartz.Impl.Triggers;

namespace QuartzImplementation;

public abstract class JobBaseImplementation<T> : IJob where T : class
{
    protected abstract Task DispacherAction();
    
    protected abstract Task LoggerDispacher();
    protected abstract Task LoggerException();
    

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await DispacherAction();
        }
        catch (Exception e)
        {
            await RetryJobOnException(context, e);
        }
    }

    protected virtual async Task RetryJobOnException(IJobExecutionContext context, Exception exception)
    {
        SimpleTriggerImpl retryTrigger = new SimpleTriggerImpl(Guid.NewGuid().ToString())
        {
            Description = $"Retry Trigger {nameof(T)}",
            RepeatCount = 0,
            JobKey = context.JobDetail.Key,
            StartTimeUtc = DateBuilder.NextGivenSecondDate(DateTime.Now, 30)
        };

        await context.Scheduler.ScheduleJob(retryTrigger);

        JobExecutionException jex = new JobExecutionException(exception, false);

        await LoggerException();

        throw jex;
    }
}