using Quartz;

namespace QuartzImplementation;

[DisallowConcurrentExecution]
public class ServiceJobTestJob : JobBaseImplementation<ServiceJobTestJob>
{
    private readonly TestingCalledService _testingCalledService;
    
    public ServiceJobTestJob(TestingCalledService testingCalledService)
    {
        _testingCalledService = testingCalledService;
    }

    protected override async Task DispacherAction()
    {
        await _testingCalledService.CalledHttpClientTesting();
    }

    protected override Task LoggerDispacher()
    {
        throw new NotImplementedException();
    }

    protected override Task LoggerException()
    {
        throw new NotImplementedException();
    }
}