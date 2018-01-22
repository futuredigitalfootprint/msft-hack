using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hack.Edge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
	            .ConfigureAppConfiguration((context, config) => config
		            .AddJsonFile("appsettings.json")
		            .AddUserSecrets<Program>()
		            .AddEnvironmentVariables()
	            )
				.ConfigureLogging((context, logging) => logging
		            .AddDebug()
		            .AddConsole())
				.UseStartup<Startup>()
                .Build();
    }
}
