using FlashPayCrawler.Crawlers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;
namespace FlashPayCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new CrawlerManager();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any,Setting.Ins.Port);
                })
                .Build();
    }
}
