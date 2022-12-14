using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace QuartzImplementation;

[ApiController]
public class ManagementJobsController : ControllerBase
{
    private readonly ISchedulerFactory _schedulerFactory;

    public ManagementJobsController(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }


    [HttpPost]
    [Route("run-job/{jobKey}")]
    public async Task<IActionResult> RunJob(string jobKey)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var exists = await scheduler.CheckExists(new JobKey(jobKey));

        if (!exists) return StatusCode(404, "Job not found");

        var job = TriggerBuilder.Create()
            .StartNow()
            .ForJob(jobKey)
            .WithPriority(10)
            .Build();

        await scheduler.ScheduleJob(job);

        return Ok();
    }


    [HttpGet]
    [Route("get-details/{jobKey}")]
    public async Task<IActionResult> GetDetailsForJob(string jobKey)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var exists = await scheduler.CheckExists(new JobKey(jobKey));

        if (!exists) return StatusCode(404, new
        {
            Message = "Job not found."
        });

        var details = await scheduler.GetJobDetail(new JobKey(jobKey));

        return Ok(new
        {
            details?.Description,
            details?.Durable,
            details?.Key,
            details?.ConcurrentExecutionDisallowed,
            details?.PersistJobDataAfterExecution
        });
    }
    
    [HttpGet]
    [Route("jobs-running")]
    public async Task<IActionResult> GetJobsRunning()
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var listJobsRunning = await scheduler.GetCurrentlyExecutingJobs();

        var factoryDetails = listJobsRunning.Select(f => new
        {
            f.Calendar,
            f.Recovering,
            f.RefireCount,
            f.PreviousFireTimeUtc,
            f.ScheduledFireTimeUtc,
            f.FireInstanceId,
            f.JobDetail.Key,
            f.JobDetail.Durable
        }).ToList();
        
        return Ok(factoryDetails);
    }
}