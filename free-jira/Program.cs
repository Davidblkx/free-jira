using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using free_jira.Server;
using free_jira.Infra;
using System.Threading.Tasks;

namespace free_jira
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
