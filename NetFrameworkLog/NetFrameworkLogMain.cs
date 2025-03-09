using NLog;
using System;

namespace NetFrameworkLog
{
    class NetFrameworkLogMain
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Программа запущена");

            try
            {
                throw new Exception("Ошибка");
            }
            catch (Exception e)
            {
                logger.Error(e, "Произошла ошибка");
            }

            logger.Info("Программа закончена");
            Console.ReadKey();
        }
    }
}