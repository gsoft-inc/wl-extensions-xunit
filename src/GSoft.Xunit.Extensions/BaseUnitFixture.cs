using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GSoft.Xunit.Extensions;

public class BaseUnitFixture : IConfigureServiceCollection, IHasXUnitOutput, IDisposable
{
    private readonly Lazy<IConfiguration> _lazyConfiguration;
    private readonly XunitLoggerProvider _loggerProvider;

    protected BaseUnitFixture()
    {
        this._lazyConfiguration = new Lazy<IConfiguration>(this.BuildConfiguration);
        this._loggerProvider = new XunitLoggerProvider();
    }

    protected IConfiguration Configuration
    {
        get => this._lazyConfiguration.Value;
    }

    private IConfiguration BuildConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        this.ConfigureConfiguration(configurationBuilder);
        return configurationBuilder.Build();
    }

    protected virtual IConfigurationBuilder ConfigureConfiguration(IConfigurationBuilder builder)
    {
        return builder;
    }

    public virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(this.Configuration);
        services.AddLogging(builder =>
        {
            var loggingSection = this.Configuration.GetSection("Logging");
            if (loggingSection.Exists())
            {
                builder.AddConfiguration(loggingSection);
            }

            builder.AddProvider(this._loggerProvider);
        });

        return services;
    }

    public void SetXunitOutput(ITestOutputHelper outputHelper)
    {
        this._loggerProvider.Output = outputHelper;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this._loggerProvider.Dispose();
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}