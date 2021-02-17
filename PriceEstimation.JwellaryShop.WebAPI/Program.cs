using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace PriceEstimation.JwellaryShop.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((host,logging) =>
            {
                logging.AddConfiguration(host.Configuration.GetSection("Logging"));
                logging.AddConsole();
            })
            .UseStartup<Startup>()
            .UseUrls("http://localhost:4000")
            .Build();
    }
}
