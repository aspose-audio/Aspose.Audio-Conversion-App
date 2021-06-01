using AudioVedio.Apis;
using AudioVedio.Pages.Models;
using AudioVedio.Pages.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Audio.Conversion
{
    public class Startup
    {
        public static readonly AudioVedioApps application = AudioVedioApps.Conversion;
        internal static string AppName
        {
            get { return application.ToString().ToRouteUrl(); }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSharedConfigParams();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration)
        {
            ServiceCollectionExtension.ConfigApp(app, env, configuration, AppName);
        }
    }
}
