using Elasticsearch.Net;
using ElasticsearchRecipes.Elastic;
using ES.Application.Services.Interfaces;
using ES.Domain.Configuration;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Domain.Extensions;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Extensions;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Application.Services
{
    public class SearchService: ISearchService
    {
        private readonly IPropertyService _propertyService;
        private readonly IManagementService _managementService;
        private readonly ElasticSearchSettings _settings;
        private readonly ElasticClient _elasticClient;

        public SearchService(
            IPropertyService propertyService,
            IManagementService managementService,
            IOptions<ElasticSearchSettings> settings,
            ElasticClientProvider provider)
        {
            _propertyService = propertyService;
            _managementService = managementService;
            _settings = settings.Value;
            _elasticClient = provider.Get();
        }

        public async Task<IEnumerable<SearchItem>> SearchAsync(string query, string market, string state, CancellationToken ct)
        {
            var request = new SearchRequestBuilder<SearchItem>()
                .Build(Indices.Index(IndexType.Management.ToString().ToLower()).And(IndexType.Property.ToString().ToLower()))
                .SetSize(_settings.Size)
                .GetRequest();

            // search
            var shouldQueryContainers = new List<QueryContainer>()
                .AddMatch(nameof(SearchItem.Name).ToCamelCase(), query)
                .AddMatchPhrase(nameof(SearchItem.Name).ToCamelCase(), query, boost: 10)
                .AddMatch(nameof(Property.FormerName).ToCamelCase(), query)
                .AddMatchPhrase(nameof(Property.FormerName).ToCamelCase(), query, boost: 10);

            // filter
            var filterQueryContainers = new List<QueryContainer>();
            if (!string.IsNullOrWhiteSpace(market))
            {
                filterQueryContainers.AddTerm(nameof(SearchItem.Market).ToCamelCase(), market);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                filterQueryContainers.AddTerm(nameof(SearchItem.State).ToCamelCase(), state);
            }

            request.Query = new BoolQuery()
            {
                Should = shouldQueryContainers,
                Filter = filterQueryContainers
            };

            //var json = _elasticClient.RequestResponseSerializer.SerializeToString(request);

            var response = await _elasticClient.SearchAsync<SearchItem>(request, ct);
            return response.Hits.Select(x => x.Source);
        }
    }
}
