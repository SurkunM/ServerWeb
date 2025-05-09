using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace LogTask;

internal class LogProgram
{
    public static void Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            using var serviceProvider = new ServiceCollection()
                .AddTransient<Runner>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Information);
                    loggingBuilder.AddNLog();
                })
                .BuildServiceProvider();

            var runner = serviceProvider.GetRequiredService<Runner>();

            runner.StartAction();
            runner.EndAction();

            throw new Exception("Тестовое исключение");
        }
        catch (Exception e)
        {
            logger.Error(e, "Ошибка в работе программы");
        }
    }
}
