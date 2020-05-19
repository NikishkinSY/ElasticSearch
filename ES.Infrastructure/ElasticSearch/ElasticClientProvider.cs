using ElasticsearchRecipes.Elastic;
using Nest;

namespace ES.Infrastructure.ElasticSearch
{
    public class ElasticClientProvider
    {
        private readonly ElasticConnectionProvider _provider;
        private ElasticClient _client { get; set; }

        public ElasticClientProvider(ElasticConnectionProvider provider)
        {
            _provider = provider;
        }

        public ElasticClient Get()
        {
            if (_client == null)
            {
                var connection = _provider.CreateConnectionSettings();
                _client = new ElasticClient(connection);
            }

            return _client;
        }
    }
}
