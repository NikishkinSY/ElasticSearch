using Elasticsearch.Net;
using ES.Domain.Configuration;
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
        private ElasticSearchSettings _settings { get; set; }

        public ElasticClientProvider(IOptions<ElasticSearchSettings> settings)
        {
            _settings = settings.Value;
        }

        public ElasticClient Get()
        {
            if (_client == null)
            {
                var pool = new StaticConnectionPool(_settings.Url.Split(',').Select(p => new Uri(p)));
                var connection = new ConnectionSettings(pool,
                    sourceSerializer: (b, s) => new JsonNetSerializer(b, s,
                        () => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                _client = new ElasticClient(connection);
            }

            return _client;
        }
    }
}