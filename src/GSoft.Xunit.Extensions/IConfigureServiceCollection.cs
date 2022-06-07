using Microsoft.Extensions.DependencyInjection;

namespace GSoft.Xunit.Extensions;

public interface IConfigureServiceCollection
{
    IServiceCollection ConfigureServices(IServiceCollection services);
}