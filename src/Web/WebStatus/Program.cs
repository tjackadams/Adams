using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace WebStatus
{
    public class Program
    {
        private static readonly string Namespace = typeof(Program).Namespace;
        private static readonly string AppName = Namespace;

        public static int Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            Log.Information("{@Configuration}", configuration["HealthChecksUI:HealthChecks:0"]);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);

                LogPackagesVersionInfo();

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
        }

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static string GetVersion(Assembly assembly)
        {
            try
            {
                return
                    $"{assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version} ({assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split()[0]})";
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void LogPackagesVersionInfo()
        {
            var assemblies = new List<Assembly>();

            foreach (var dependencyName in typeof(Program).Assembly.GetReferencedAssemblies())
            {
                try
                {
                    // Try to load the referenced assembly...
                    assemblies.Add(Assembly.Load(dependencyName));
                }
                catch
                {
                    // Failed to load assembly. Skip it.
                }
            }

            var versionList = assemblies.Select(a => $"-{a.GetName().Name} - {GetVersion(a)}").OrderBy(value => value);

            Log.Logger.ForContext("PackageVersions", string.Join("\n", versionList))
                .Information("Package versions ({ApplicationContext})", AppName);
        }
    }
}