using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ShareGate.Extensions.Xunit;

public abstract class BaseUnitTest : BaseUnitTest<EmptyUnitFixture>
{
    protected BaseUnitTest(ITestOutputHelper testOutputHelper)
        : base(new EmptyUnitFixture(), testOutputHelper)
    {
    }
}

public abstract class BaseUnitTest<TFixture> : IClassFixture<TFixture>, IDisposable
    where TFixture : class
{
    private readonly ServiceCollection _services;
    private readonly Lazy<ServiceProvider> _lazyServiceProvider;
    private readonly Lazy<ILogger> _lazyLogger;

    protected BaseUnitTest(TFixture fixture, ITestOutputHelper testOutputHelper)
    {
        this._services = new ServiceCollection();
        this._lazyServiceProvider = new Lazy<ServiceProvider>(this.CreateServiceProvider);
        this._lazyLogger = new Lazy<ILogger>(this.CreateLogger);

        if (fixture is IHasXUnitOutput xunitOutputFixture)
        {
            xunitOutputFixture.SetXunitOutput(testOutputHelper);
        }

        this.Fixture = fixture;
    }

    protected TFixture Fixture { get; }

    protected ILogger Logger
    {
        get => this._lazyLogger.Value;
    }

    protected virtual IServiceProvider Services
    {
        get => this._lazyServiceProvider.Value;
    }

    private ServiceProvider CreateServiceProvider()
    {
        if (this.Fixture is IConfigureServiceCollection serviceCollectionConfigurator)
        {
            serviceCollectionConfigurator.ConfigureServices(this._services);
        }

        return this._services.BuildServiceProvider();
    }

    private ILogger CreateLogger()
    {
        var loggerType = typeof(ILogger<>).MakeGenericType(this.GetType());
        return (ILogger)this.Services.GetRequiredService(loggerType);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this._lazyServiceProvider.IsValueCreated)
            {
                this._lazyServiceProvider.Value.Dispose();
            }
        }
    }
}