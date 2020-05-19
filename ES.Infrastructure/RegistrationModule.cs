using ElasticsearchRecipes.Elastic;
using ES.Infrastructure.AWS;
using ES.Infrastructure.ElasticSearch;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Infrastructure
{
    public static class RegistrationModule
    {
        public static IServiceCollection RegisterInfrastructureModule(this IServiceCollection services)
        {
            return services
                .AddTransient<AWSConnectionProvider>()
                .AddTransient<SerilogConfiguration>()
                .AddTransient<ElasticConnectionProvider>()
                .AddSingleton<ElasticClientProvider>();
        }
    }
}
