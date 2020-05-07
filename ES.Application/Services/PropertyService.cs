using ElasticsearchRecipes.Elastic;
using ES.Application.Services.Interfaces;
using ES.Domain.Configuration;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Domain.Extensions;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Extensions;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Application.Services
{
    public class PropertyService: IPropertyService
    {
        private readonly ElasticClient _elasticClient;
        private readonly ElasticSearchSettings _settings;

        public PropertyService(
            ElasticClientProvider provider,
            IOptions<ElasticSearchSettings> settings)
        {
            _elasticClient = provider.Get();
            _settings = settings.Value;
        }

        public async Task<IEnumerable<Property>> SearchAsync(string query, string market, string state)
        {
            var request = new SearchRequestBuilder<Property>()
                .Build(typeof(Property))
                .SetSize(_settings.Size)
                .GetRequest();

            // search
            var shouldQueryContainers = new List<QueryContainer>()
                .AddMatch(nameof(Property.Name).ToCamelCase(), query)
                .AddMatchPhrase(nameof(Property.Name).ToCamelCase(), query, boost: 10)
                .AddMatch(nameof(Property.FormerName).ToCamelCase(), query)
                .AddMatchPhrase(nameof(Property.FormerName).ToCamelCase(), query, boost: 10);

            // filter
            var filterQueryContainers = new List<QueryContainer>();
            if (!string.IsNullOrWhiteSpace(market))
            {
                filterQueryContainers.AddTerm(nameof(Management.Market).ToCamelCase(), market);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                filterQueryContainers.AddTerm(nameof(Management.State).ToCamelCase(), state);
            }

            request.Query = new BoolQuery()
            {
                Should = shouldQueryContainers,
                Filter = filterQueryContainers
            };

            //var json = _elasticClient.RequestResponseSerializer.SerializeToString(request);

            var response = await _elasticClient.SearchAsync<Property>(request);
            return response.Hits.Select(x => x.Source);
        }
    }
}
