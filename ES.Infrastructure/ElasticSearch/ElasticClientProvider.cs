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
    public class ElasticClientProvider
    {
        private ElasticClient _client { get; set; }
        private ElasticSearchSettings _esSettings { get; set; }
        private AWSConnectionProvider _awsProvider { get; set; }

        public ElasticClientProvider(
            IOptions<ElasticSearchSettings> esSettings,
            AWSConnectionProvider awsProvider)
        {
            _esSettings = esSettings.Value;
            _awsProvider = awsProvider;
        }

        public ElasticClient Get()
        {
            if (_client == null)
            {
                var httpConnection = _awsProvider.Get();
                var pool = new StaticConnectionPool(_esSettings.Url.Split(',').Select(p => new Uri(p)));
                var connection = new ConnectionSettings(pool, httpConnection,
                    sourceSerializer: (b, s) => new JsonNetSerializer(b, s,
                        () => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                _client = new ElasticClient(connection);
            }

            return _client;
        }
    }
}