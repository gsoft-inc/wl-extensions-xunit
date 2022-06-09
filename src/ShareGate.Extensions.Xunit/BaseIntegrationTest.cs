namespace ShareGate.Extensions.Xunit;

public abstract class BaseIntegrationTest : BaseIntegrationTest<EmptyIntegrationFixture>
{
    protected BaseIntegrationTest(ITestOutputHelper testOutputHelper)
        : base(new EmptyIntegrationFixture(), testOutputHelper)
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