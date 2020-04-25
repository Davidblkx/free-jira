using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using FreeJira.Server;
using FreeJira.Infra;
using System.Threading.Tasks;

namespace FreeJira
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var settings = await FreeJiraSettings.GetSettings();
            if (args.Contains("--server") || (settings.StartupMode == StartupMode.Server) && !args.Contains("--terminal")) {
                CreateHostBuilder(args, settings.ServerPort).Build().Run();
                return 0;
            } else {
                return await Terminal.TerminalHandler.HandleArgs(args);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, int port) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls($"http://localhost:{port}");
                });
    }
}
