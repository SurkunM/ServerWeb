using System;
using System.Configuration;

namespace ConfigurationFile
{
    internal class ConfigurationFileProgram
    {
        static void Main(string[] args)
        {
            var siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
            Console.WriteLine(siteUrl);
        }
    }
}