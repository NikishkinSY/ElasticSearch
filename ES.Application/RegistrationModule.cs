using ES.Application.Services;
using ES.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Application
{
    public static class RegistrationModule
    {
        public static IServiceCollection RegisterApplicationModule(this IServiceCollection services)
        {
            return services
                .AddScoped<IIndexService, IndexService>()
                .AddScoped<ISearchService, SearchService>();
        }
    }
}
