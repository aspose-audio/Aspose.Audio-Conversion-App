using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Serilog;
using AudioVedio.Apis.Services;
using AudioVedio.Apis.Filters;
using Microsoft.Extensions.Configuration;
using Elastic.Apm.AspNetCore;
using Serilog.Sinks.Elasticsearch;
using Serilog.Formatting.Elasticsearch;
using Serilog.Events;

namespace AudioVedio.Apis
{
    public static class ServiceCollectionExtension
    {
        public static long MaximumUploadFileSize { get; } = 150 * 1024 * 1024;
        public static void AddSharedConfigParams(this IServiceCollection services)
        {
            services.AddMvc(options => { options.Filters.Add<ApiExceptionFilter>(); options.Filters.Add<ApiSessionIdActionFilter>(); })
             .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            AppRegister.RegistGlobalAudioVedio();

            services.AddScoped<IStorage, AmazonS3CloudStorage>();
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = MaximumUploadFileSize;
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = MaximumUploadFileSize;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = MaximumUploadFileSize;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
        }

        public static void ConfigApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration,string AppName) {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseElasticApm(configuration);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                {
                    endpoints.MapControllerRoute("Index", "/", new { controller = AppName, action = "index" });
                }
                else
                {
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Healthy");
                    });
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName, "audio/" + AppName,
                    new { controller = AppName, action = "index" });
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName + "Format", "audio/" + AppName + "/{fileformat}",
                    new { controller = AppName, action = "index", fileformat = "" });
            });

            var apiBasePath = $"audio/{AppName}/api/{AppName}";
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName + "DownloadApi", apiBasePath + "/storage/download",
                    new { controller = "Storage", action = "Download" });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName + "UploadApi", apiBasePath + "/storage/upload",
                    new { controller = "Storage", action = "Upload" });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName + "ReportErrorApi", apiBasePath + "/report/error",
                    new { controller = "Common", action = "Error" });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName + "FeedbackApi", apiBasePath + "/email/feedback",
                    new { controller = "Common", action = "SendFeedback" });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(AppName + "SendFileApi", apiBasePath + "/email/send-file",
                    new { controller = "Common", action = "SendFile" });
            });

            var contentPath = "";
            if (env.IsDevelopment())
            {
                contentPath = Path.Combine(env.ContentRootPath, "../Audio.Pages/Resources");
            }
            else {
                contentPath = Path.Combine(env.ContentRootPath, "Resources");
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(contentPath),
                RequestPath = "/audio/" + AppName + "/content"
            });
        }

        public static IHostBuilder AddSeriLogger(this IHostBuilder hostBuilder,string appName) {
            hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration = loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                loggerConfiguration.WriteTo.Console();
                loggerConfiguration.MinimumLevel.Warning();
                //loggerConfiguration.WriteTo.File(path:Path.Combine(AppContext.BaseDirectory,"log"));
                var elasticsearchSinkOptions = new ElasticsearchSinkOptions(new Uri(AppConfig.EsUrl))
                {
                    IndexFormat = "audio-" + appName + "-app-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true,
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                                   EmitEventFailureHandling.RaiseCallback |
                                                   EmitEventFailureHandling.ThrowException,
                    ModifyConnectionSettings = x => x.BasicAuthentication(
                        AppConfig.EsLogin,
                        AppConfig.EsPassword),
                    MinimumLogEventLevel=LogEventLevel.Warning,
                };
                loggerConfiguration.WriteTo.Elasticsearch(elasticsearchSinkOptions);
                            
            });
            return hostBuilder;
        }
    }
}
