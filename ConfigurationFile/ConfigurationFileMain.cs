using System;
using System.Configuration;

namespace ConfigurationFile
{
    internal class ConfigurationFileMain
    {
        static void Main(string[] args)
        {
            var siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
            Console.WriteLine($"{siteUrl}");
        }
    }
}