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
            IOptions<ElasticSearchSettings> esSettings,
            AWSConnectionProvider awsProvider)
        {
            _esSettings = esSettings.Value;
            _awsProvider = awsProvider;
        }

        public ConnectionSettings GetConnectionSettings()
        {
            var httpConnection = _awsProvider.Get();
            var pool = GetConnectionPool();
            return new ConnectionSettings(pool, httpConnection,
                sourceSerializer: (b, s) => new JsonNetSerializer(b, s,
                    () => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
        }

        public ConnectionConfiguration GetConnectionConfiguration()
        {
            var httpConnection = _awsProvider.Get();
            var pool = GetConnectionPool();
            return new ConnectionConfiguration(pool, httpConnection);
        }

        private StaticConnectionPool GetConnectionPool()
        {
            return new StaticConnectionPool(_esSettings.Url.Split(',').Select(p => new Uri(p)));
        }
    }
}