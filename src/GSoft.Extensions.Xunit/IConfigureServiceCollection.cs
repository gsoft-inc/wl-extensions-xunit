using Microsoft.Extensions.DependencyInjection;

namespace GSoft.Extensions.Xunit;

public interface IConfigureServiceCollection
{
    IServiceCollection ConfigureServices(IServiceCollection services);
}