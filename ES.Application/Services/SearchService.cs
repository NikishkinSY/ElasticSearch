using AutoMapper;
using Elasticsearch.Net;
using ElasticsearchRecipes.Elastic;
using ES.Application.ElasticSearch.Entities;
using ES.Application.Services.Interfaces;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Domain.Exceptions;
using ES.Domain.Extensions;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Extensions;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Application.Services
{
    public class SearchService: ISearchService
    {
        private readonly ElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public SearchService(
            ElasticConnectionProvider provider,
            IMapper mapper)
        {
            var connection = provider.Get();
            _elasticClient = new ElasticClient(connection);
            _mapper = mapper;
        }

        public async Task<IEnumerable<BaseItem>> SearchAsync(
            string query, 
            ICollection<string> markets, 
            ICollection<string> states, 
            int size, 
            CancellationToken ct = default)
        {
            var request = new SearchRequestBuilder<BaseItem>()
                .Build(Indices.Index(IndexType.Management.ToString().ToLower()).And(IndexType.Property.ToString().ToLower()))
                .SetSize(size)
                .GetRequest();

            // search
            var shouldQueryContainers = new List<QueryContainer>()
                .AddMultiMatch(new[] {
                    nameof(PropertyES.Name).ToCamelCase(),
                    nameof(PropertyES.FormerName).ToCamelCase(),
                    nameof(PropertyES.StreetAddress).ToCamelCase(),
                    nameof(PropertyES.City).ToCamelCase()
                }, query, fuzzyTranspositions: true)
                .AddMatchPhrase(nameof(PropertyES.Name).ToCamelCase(), query, boost: 10)
                .AddMatchPhrase(nameof(PropertyES.FormerName).ToCamelCase(), query, boost: 10);

            // filter
            var filterQueryContainers = new List<QueryContainer>();
            markets = markets.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (markets.Any())
            {
                filterQueryContainers.AddTerms(nameof(PropertyES.Market).ToCamelCase(), markets);
            }

            states = states.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (states.Any())
            {
                filterQueryContainers.AddTerms(nameof(PropertyES.State).ToCamelCase(), states);
            }

            request.Query = new BoolQuery()
            {
                Should = shouldQueryContainers,
                Filter = filterQueryContainers,
                MinimumShouldMatch = 1
            };

            // get json request to use in kibana
            //var json = _elasticClient.RequestResponseSerializer.SerializeToString(request);

            var response = await _elasticClient.SearchAsync<object>(request, ct);
            if (!response.IsValid)
            {
                throw new ElasticSearchException(response.ServerError?.Error?.Reason, response.OriginalException);
            }

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
