using ApplicationTemplate.Config;
using ApplicationTemplate.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationTemplate
{
    public class Program
    {
        /// <summary>
        ///     Configuration for the application
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        ///     Entry point of the project.
        ///     Notice the method is returning async task instead of void in order to support async programming.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                                        .AddCommandLine(args)
                                        .AddEnvironmentVariables()
                                        .Build();

                // configure serilog
                Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                                                      .Enrich.FromLogContext()
                                                      .Enrich.WithMachineName()
                                                      .CreateLogger();
                Log.Information("Starting up...");

                // Create service collection and configure our services
                var services = ConfigureServices();
                // Generate a provider
                var serviceProvider = services.BuildServiceProvider();

                // Kick off the actual application
                ApplicationRunResponse response = await serviceProvider.GetService<Application>().Run();

                Log.Information("Shutting down...");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        ///     Configure the services using ASP.NET CORE built-in Dependency Injection.
        /// </summary>
        /// <returns></returns>
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            // register the services
            
            // Configuration should be singleton as the entire application should use one
            services.AddSingleton(Configuration);
            // for strongly typed options to be injected as IOption<T> in constructors
            services.AddOptions();
            // Configure EmailSettings so IOption<EmailSettings> can be injected 
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            // Register the actual application entry point
            services.AddTransient<Application>();

            return services;
        }
    }
}
