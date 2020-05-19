using ElasticsearchRecipes.Elastic;
using ES.Infrastructure.AWS;
using ES.Infrastructure.AWS.Interfaces;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Infrastructure
{
    public static class RegistrationModule
    {
        public static IServiceCollection RegisterInfrastructureModule(this IServiceCollection services)
        {
            return services
                .AddTransient<IAWSConnectionProvider, AWSConnectionProvider>()
                .AddTransient<IElasticConnectionProvider, ElasticConnectionProvider>()
                .AddSingleton<IElasticClientProvider, ElasticClientProvider>();
        }
    }
}
