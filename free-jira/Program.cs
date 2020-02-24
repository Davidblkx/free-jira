using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using free_jira.Server;

namespace free_jira
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (Terminal.TerminalHandler.HandleArgs(args)) {
                Console.WriteLine("Hello");
            } else {
                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
