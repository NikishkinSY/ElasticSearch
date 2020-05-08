using ElasticsearchRecipes.Elastic;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Infrastructure
{
    public static class RegistrationModule
    {
        public static IServiceCollection RegisterInfrastructureModule(this IServiceCollection services)
        {
            return services
                .AddScoped<ElasticClientProvider>();
        }
    }
}
