using Microsoft.Extensions.Logging;

namespace LogTask;

public class Runner
{
    private readonly ILogger<Runner> _logger;

    public Runner(ILogger<Runner> logger)
    {
        _logger = logger;
    }

    public void StartAction()
    {
        _logger.LogWarning("Программа запущена");
    }

    public void EndAction()
    {
        _logger.LogInformation("Программа завершена");
    }
}
