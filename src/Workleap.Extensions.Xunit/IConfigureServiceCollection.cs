using Microsoft.Extensions.DependencyInjection;

namespace Workleap.Extensions.Xunit;

public interface IConfigureServiceCollection
{
    IServiceCollection ConfigureServices(IServiceCollection services);
}