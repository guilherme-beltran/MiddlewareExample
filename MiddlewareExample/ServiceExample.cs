using MiddlewareExample;

internal sealed class ServiceExample : IServiceExample
{
    private readonly ILogger<ServiceExample> _logger;

    public ServiceExample(ILogger<ServiceExample> logger)
    {
        _logger = logger;
    }

    public Task DoSomething()
    {
        _logger.LogInformation("Service Example has been accessed");
        return Task.CompletedTask;
    }
}