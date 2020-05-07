using ElasticsearchRecipes.Elastic;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Domain.Exceptions;
using ES.Infrastructure.ElasticSearch.Extensions;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Infrastructure.ElasticSearch
{
    public class IndexService: IIndexService
    {
        private readonly ElasticClient _elasticClient;

        public IndexService(ElasticClientProvider provider)
        {
            _elasticClient = provider.Get();
        }

        public async Task<bool> CreateIndex(IndexType type, CancellationToken ct)
        {
            var response = await _elasticClient.Indices.CreateAsync(type.ToString().ToLower(), c => c.CreateIndexSettingAndMapping(type), ct);
            return ErrorHandler(response);
        }

        public async Task<bool> IndexDocuments(IndexType type, string json, CancellationToken ct)
        {
            BulkResponse response = null;
            switch (type)
            {
                case IndexType.Management:
                    var managements = JsonConvert.DeserializeObject<List<Management>>(json);
                    response = await _elasticClient.IndexManyAsync(managements, type.ToString().ToLower(), ct);
                    break;
                case IndexType.Property:
                    var properties = JsonConvert.DeserializeObject<List<Property>>(json);
                    response = await _elasticClient.IndexManyAsync(properties, type.ToString().ToLower(), ct);
                    break;
            }

            return ErrorHandler(response);
        }

        public async Task<bool> DeleteIndex(IndexType type, CancellationToken ct)
        {
            var response = await _elasticClient.Indices.DeleteAsync(type.ToString().ToLower(), ct: ct);
            return ErrorHandler(response);
        }

        private bool ErrorHandler(ResponseBase response)
        {
            if (!response.IsValid)
            {
                throw new ElasticSearchException(response.ServerError.Error.Reason);
            }

            return response.IsValid;
        }
    }
}
