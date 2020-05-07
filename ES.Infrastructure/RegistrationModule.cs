using ElasticsearchRecipes.Elastic;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Infrastructure
{
    public static class RegistrationModule
    {
        public static IServiceCollection RegisterInfrastructureModule(this IServiceCollection services)
        {
            return services
                .AddScoped<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>()
                .AddScoped<IIndexService, IndexService>()
                .AddScoped<ElasticClientProvider>();
        }
    }
}
