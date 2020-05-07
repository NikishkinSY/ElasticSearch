using AutoMapper;
using Elasticsearch.Net;
using ElasticsearchRecipes.Elastic;
using ES.Application.Services.Interfaces;
using ES.Domain.Configuration;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Domain.Extensions;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Entities;
using ES.Infrastructure.ElasticSearch.Extensions;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Application.Services
{
    public class SearchService: ISearchService
    {
        private readonly ElasticSearchSettings _settings;
        private readonly ElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public SearchService(
            IOptions<ElasticSearchSettings> settings,
            ElasticClientProvider provider,
            IMapper mapper)
        {
            _settings = settings.Value;
            _elasticClient = provider.Get();
            _mapper = mapper;
        }

        public async Task<IEnumerable<BaseItem>> SearchAsync(string query, string market, string state, CancellationToken ct = default)
        {
            var request = new SearchRequestBuilder<BaseItem>()
                .Build(Indices.Index(IndexType.Management.ToString().ToLower()).And(IndexType.Property.ToString().ToLower()))
                .SetSize(_settings.Size)
                .GetRequest();

            // search
            var shouldQueryContainers = new List<QueryContainer>()
                .AddMultiMatch(new[] {
                    nameof(PropertyES.Name).ToCamelCase(),
                    nameof(PropertyES.FormerName).ToCamelCase(),
                    nameof(PropertyES.StreetAddress).ToCamelCase(),
                    nameof(PropertyES.City).ToCamelCase()
                }, query)
                .AddMatchPhrase(nameof(PropertyES.Name).ToCamelCase(), query, boost: 10)
                .AddMatchPhrase(nameof(PropertyES.FormerName).ToCamelCase(), query, boost: 10);

            // filter
            var filterQueryContainers = new List<QueryContainer>();
            if (!string.IsNullOrWhiteSpace(market))
            {
                filterQueryContainers.AddTerm(nameof(PropertyES.Market).ToCamelCase(), market);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                filterQueryContainers.AddTerm(nameof(PropertyES.State).ToCamelCase(), state);
            }

            request.Query = new BoolQuery()
            {
                Should = shouldQueryContainers,
                Filter = filterQueryContainers,
                MinimumShouldMatch = 1
            };

            //var json = _elasticClient.RequestResponseSerializer.SerializeToString(request);

            var response = await _elasticClient.SearchAsync<object>(request, ct);

            var result = new List<BaseItem>();
            foreach (var doc in response.Documents)
            {
                if (doc is ManagementES)
                    result.Add(_mapper.Map<Management>(doc));
                else if (doc is PropertyES)
                    result.Add(_mapper.Map<Property>(doc));
            }

            return result;
        }
    }
}
