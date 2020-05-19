using ElasticsearchRecipes.Elastic;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace ES.Infrastructure
{
    public class SerilogConfiguration
    {
        private readonly ElasticConnectionProvider _elasticConnectionProvider;

        public SerilogConfiguration(ElasticConnectionProvider elasticConnectionProvider)
        {
            _elasticConnectionProvider = elasticConnectionProvider;
        }

        public void UseElasticSearch()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions()
                {
                    ModifyConnectionSettings = conn =>
                    {
                        return _elasticConnectionProvider.CreateConnectionConfiguration();
                    },
                    IndexFormat = "{0:yyyy.MM}",
                })
                .CreateLogger();
        }
    }
}
