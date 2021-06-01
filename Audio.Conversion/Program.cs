using AudioVedio.Apis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Audio.Conversion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string appId = "Audio " + Startup.AppName;
            ILogger logger = null;
            try
            {
                var host = CreateHostBuilder(args).Build();
                logger = host.Services.GetService<ILogger<Program>>();
                logger.ApplicationStarting(appId);
                logger.ApplicationVersion(DateTime.Now.ToString());
                logger.ApplicationEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                host.Run();
                logger.ApplicationShutdown(appId);
            }
            catch (Exception ex)
            {
                logger.ApplicationTerminatedException(appId, ex);
            }
            finally
            {
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddSeriLogger(Startup.AppName)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
