using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace ES.Infrastructure
{
    public static class SerilogConfigurationExtensions
    {
        public static IApplicationBuilder UseElasticSearch(this IApplicationBuilder app, IElasticConnectionProvider elasticConnectionProvider, string indexFormat)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions()
                {
                    ModifyConnectionSettings = conn =>
                    {
                        return elasticConnectionProvider.CreateConnectionConfiguration();
                    },
                    IndexFormat = indexFormat,
                })
                .CreateLogger();

            return app;
        }
    }
}
