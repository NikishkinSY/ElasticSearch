using Elasticsearch.Net;
using ES.Domain.Configuration;
using ES.Infrastructure.AWS;
using ES.Infrastructure.AWS.Interfaces;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.Options;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ElasticsearchRecipes.Elastic
{
    public class ElasticConnectionProvider: IElasticConnectionProvider
    {
        private readonly ElasticSearchSettings _esSettings;
        private readonly IAWSConnectionProvider _awsProvider;

        public ElasticConnectionProvider(
            IOptionsSnapshot<ElasticSearchSettings> esSettings,
            IAWSConnectionProvider awsProvider)
        {
            _esSettings = esSettings.Value;
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