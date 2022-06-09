using Microsoft.Extensions.DependencyInjection;

namespace ShareGate.Extensions.Xunit;

public interface IConfigureServiceCollection
{
    IServiceCollection ConfigureServices(IServiceCollection services);
}