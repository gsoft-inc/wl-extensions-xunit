# GSoft.Xunit.Extensions

An opinionated library that provides base unit test and fixture classes based on the `Microsoft.Extensions.*` packages used by modern .NET applications.


## Getting started

There are base classes for **unit** and **integration tests**. Each test method has its own [service collection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0) configured through a class fixture.


### Unit tests

Create a test class that extends `BaseUnitTest<>` and accepts a class fixture that extends `BaseUnitFixture`.

In the fixture class, you can:
* Override `ConfigureServices(services)` to add, remove or update dependencies for each test method.
* Override `ConfigureConfiguration(builder)` to makes changes to the generated shared `IConfiguration`.
* Access the generated `IConfiguration` through `this.Configuration`.

In the test class, you can:
* Access the fixture through `this.Fixture`.
* Access the .NET logger through `this.Logger` - it is connected to the Xunit's `ITestOutputHelper`.
* Access the `IServiceProvider` which has been configured by the fixture through `this.Services`.

By default, unit tests come with an xunit-connected `ILogger` and an empty `IConfiguration`.

### Integration tests

Create a test class that extends `BaseIntegrationTest<>` and accepts a class fixture that extends `BaseIntegrationFixture`.
They both inherit from their respective `BaseUnit*` class.

* `BaseIntegrationFixture` adds a default `IHostEnvironment` where its environment name is:
  * `Local` by default,
  * `Test` on CI environments,
  * overrideable by defining a `DOTNET_ENVIRONMENT` environment variable, such as in .NET modern applications.

* `BaseIntegrationFixture` adds `appsettings.json` and `appsettings.{environment}.json` optional configuration providers and also an environment variable configuration provider.


### Example

```csharp
public class MyUnitTests : BaseUnitTest<MyUnitFixture>
{
    public MyUnitTests(MyUnitFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }

    [Fact]
    public void Some_Test_Works()
    {
        var myClass = this.Services.GetRequiredService<MyClass>();
        myClass.DoWork();
    }
}

public class MyUnitFixture : BaseUnitFixture
{
    protected override IConfigurationBuilder ConfigureConfiguration(IConfigurationBuilder builder)
    {
        // Executed once per fixture instance
        return base.ConfigureConfiguration(builder).AddInMemoryCollection(new Dictionary<string, string>
        {
            ["My:Config:Variable"] = "foo",
        });
        
        // In an integration fixture, you could add concrete configuration providers, such as:
        // builder.AddAzureKeyVault(...);
    }

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        // Executed for each test method
        return base.ConfigureServices(services)
            .AddTransient<MyClass>()
            .AddTransient<IDependency>(new MyFakeDependency());
    }
}
```


## License

Copyright © 2022, GSoft inc. This code is licensed under the Apache License, Version 2.0. You may obtain a copy of this license at https://github.com/gsoft-inc/gsoft-license/blob/master/LICENSE.
