using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Workleap.Extensions.Xunit;

public abstract class BaseIntegrationFixture : BaseUnitFixture
{
    protected BaseIntegrationFixture()
    {
        this.Environment = new TestHostEnvironment();
    }

    protected virtual IHostEnvironment Environment { get; }

    protected override IConfigurationBuilder ConfigureConfiguration(IConfigurationBuilder builder)
    {
        base.ConfigureConfiguration(builder);

        builder.SetBasePath(AppContext.BaseDirectory);
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        builder.AddJsonFile("appsettings." + this.Environment.EnvironmentName + ".json", optional: true, reloadOnChange: false);
        builder.AddEnvironmentVariables();

        return builder;
    }

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.TryAddSingleton(this.Environment);

        return services;
    }

    private sealed class TestHostEnvironment : IHostEnvironment
    {
        private static readonly bool IsCI = IsContinuousIntegrationServer();

        public TestHostEnvironment()
        {
            this.EnvironmentName = DetermineEnvironmentName();
            this.ApplicationName = "Tests";
            this.ContentRootPath = AppContext.BaseDirectory;
            this.ContentRootFileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
        }

        public string EnvironmentName { get; set; }

        public string ApplicationName { get; set; }

        public string ContentRootPath { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }

        private static string DetermineEnvironmentName()
        {
            // Similar to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0#environments
            if (System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") is { Length: > 0 } environmentName)
            {
                return environmentName;
            }

            return IsCI ? "Test" : "Local";
        }

        private static bool IsContinuousIntegrationServer()
        {
            // The following environment variables are defined by continuous integration servers
            // Inspired from https://github.com/watson/ci-info/blob/v3.3.1/vendors.json
            var envNames = new[]
            {
                "SYSTEM_TEAMFOUNDATIONCOLLECTIONURI", // Azure Pipelines
                "GITHUB_ACTIONS", // GitHub Actions
                "TEAMCITY", // TeamCity
            };

            return envNames.Any(x => !string.IsNullOrWhiteSpace(System.Environment.GetEnvironmentVariable(x)));
        }
    }
}