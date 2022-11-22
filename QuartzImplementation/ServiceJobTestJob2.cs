
using Quartz;

namespace QuartzImplementation;

public class ServiceJobTestJob2 : JobBaseImplementation<ServiceJobTestJob2>
{
    protected override async Task DispacherAction()
    {
        Console.WriteLine("Run service job 2");

        await Task.Delay(5000);
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