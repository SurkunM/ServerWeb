using System.Configuration;

namespace ConfigurationFile
{
    internal class ConfigurationFileMain
    {
        static void Main(string[] args)
        {
            string value = ConfigurationManager.AppSettings["TestPath"];
        }
    }
}