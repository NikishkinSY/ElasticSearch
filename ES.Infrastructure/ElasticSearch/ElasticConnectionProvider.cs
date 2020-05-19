using Elasticsearch.Net;
using ES.Domain.Configuration;
using ES.Infrastructure.AWS;
using Microsoft.Extensions.Options;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ElasticsearchRecipes.Elastic
{
    public class ElasticConnectionProvider
    {
        private readonly ElasticSearchSettings _esSettings;
        private readonly AWSConnectionProvider _awsProvider;

        public ElasticConnectionProvider(
            IOptionsMonitor<ElasticSearchSettings> esSettings,
            AWSConnectionProvider awsProvider)
        {
            _esSettings = esSettings.CurrentValue;
            _awsProvider = awsProvider;
        }

        public ConnectionSettings CreateConnectionSettings()
        {
            var httpConnection = _awsProvider.Create();
            var pool = CreateConnectionPool();
            return new ConnectionSettings(pool, httpConnection,
                sourceSerializer: (b, s) => new JsonNetSerializer(b, s,
                    () => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
        }

        public ConnectionConfiguration CreateConnectionConfiguration()
        {
            var httpConnection = _awsProvider.Create();
            var pool = CreateConnectionPool();
            return new ConnectionConfiguration(pool, httpConnection);
        }

        private IConnectionPool CreateConnectionPool()
        {
            return new StaticConnectionPool(_esSettings.Url.Split(',').Select(p => new Uri(p)));
        }
    }
}