using Quartz;

namespace QuartzImplementation;

public static class ConfigurationsQuartz
{
    private static void AddJobAndTrigger<T>(
        this IServiceCollectionQuartzConfigurator quartz,
        IConfiguration config)
        where T : IJob
    {
        var jobName = typeof(T).Name;
        var configKey = $"Quartz:{jobName}";
        var cronSchedule = config[configKey];

        if (string.IsNullOrEmpty(cronSchedule))
            throw new Exception($"There is no cron configured for the job {jobName}");

        var jobKey = new JobKey(jobName);
        
        quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(cronSchedule));
    }


    public static void SettingJobs(IServiceCollectionQuartzConfigurator configuratorJobs,IConfiguration configuration)
    {
        configuratorJobs.UseMicrosoftDependencyInjectionJobFactory();
        
        configuratorJobs.AddJobAndTrigger<ServiceJobTestJob>(configuration);
        configuratorJobs.AddJobAndTrigger<ServiceJobTestJob2>(configuration);
    }
}