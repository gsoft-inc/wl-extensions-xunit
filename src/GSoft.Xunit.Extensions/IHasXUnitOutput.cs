namespace GSoft.Xunit.Extensions;

public interface IHasXUnitOutput
{
    void SetXunitOutput(ITestOutputHelper outputHelper);
}