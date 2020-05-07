using ElasticsearchRecipes.Elastic;
using ES.Domain.Configuration;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Domain.Extensions;
using ES.Infrastructure.ElasticSearch.Extensions;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Infrastructure.ElasticSearch
{
    public class ManagementService: IManagementService
    {
        private readonly ElasticClient _elasticClient;
        private readonly ElasticSearchSettings _settings;

        public ManagementService(
            ElasticClientProvider provider,
            IOptions<ElasticSearchSettings> settings)
        {
            _elasticClient = provider.Get();
            _settings = settings.Value;
        }

        public async Task<IEnumerable<Management>> SearchAsync(string query, string market, string state)
        {
            var request = new SearchRequestBuilder<Management>()
                .Build(typeof(Management))
                .SetSize(_settings.Size)
                .GetRequest();

            // search
            var shouldQueryContainers = new List<QueryContainer>()
                .AddMatch(nameof(Management.Name).ToCamelCase(), query)
                .AddMatchPhrase(nameof(Management.Name).ToCamelCase(), query, boost: 10);

            // filter
            var filterQueryContainers = new List<QueryContainer>();
            if (!string.IsNullOrWhiteSpace(market))
            {
                filterQueryContainers.AddTerm(nameof(Management.Market), market);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                filterQueryContainers.AddTerm(nameof(Management.State), state);
            }

            request.Query = new BoolQuery()
            {
                Should = shouldQueryContainers,
                Filter = filterQueryContainers
            };

            //var json = _elasticClient.RequestResponseSerializer.SerializeToString(request);

            var response = await _elasticClient.SearchAsync<Management>(request);
            return response.Hits.Select(x => x.Source);
        }
    }
}
