using ElasticsearchRecipes.Elastic;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Nest;

namespace ES.Infrastructure.ElasticSearch
{
    public class ElasticClientProvider: IElasticClientProvider
    {
        private readonly IElasticConnectionProvider _provider;
        private ElasticClient _client { get; set; }

        public ElasticClientProvider(IElasticConnectionProvider provider)
        {
            _provider = provider;
        }

        public IElasticClient Get()
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
