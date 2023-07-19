namespace Workleap.Extensions.Xunit;

public abstract class BaseIntegrationTest : BaseIntegrationTest<EmptyIntegrationFixture>
{
    protected BaseIntegrationTest(EmptyIntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }
}

public abstract class BaseIntegrationTest<TFixture> : BaseUnitTest<TFixture>
    where TFixture : class
{
    protected BaseIntegrationTest(TFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }
}