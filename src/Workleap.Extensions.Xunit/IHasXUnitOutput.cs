﻿namespace Workleap.Extensions.Xunit;

public interface IHasXUnitOutput
{
    void SetXunitOutput(ITestOutputHelper outputHelper);
}