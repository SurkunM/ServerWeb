using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace LogTask;

internal class LogProgram
{
    public static void Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            using var servicesProvider = new ServiceCollection()
                .AddTransient<Runner>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                    loggingBuilder.AddNLog();
                }).BuildServiceProvider();

            var runner = servicesProvider.GetRequiredService<Runner>();

            runner.StartAction();
            runner.EndAction();

            throw new Exception("Тестовое исключение");
        }
        catch (Exception e)
        {
            logger.Error($"Ошибка! {e}");
        }
    }
}
