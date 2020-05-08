using ElasticsearchRecipes.Elastic;
using ES.Application.ElasticSearch.CreateIndexExtensions;
using ES.Application.ElasticSearch.Entities;
using ES.Domain.Enums;
using ES.Domain.Exceptions;
using Nest;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Application.Services.Interfaces
{
    public class IndexService: IIndexService
    {
        private readonly ElasticClient _elasticClient;

        public IndexService(ElasticClientProvider provider)
        {
            _elasticClient = provider.Get();
        }

        public async Task<bool> CreateIndex(IndexType type, CancellationToken ct = default)
        {
            var response = await _elasticClient.Indices.CreateAsync(type.ToString().ToLower(), c => c.CreateIndexSettingAndMapping(type), ct);
            return ErrorHandler(response);
        }

        public async Task<bool> IndexDocuments(IndexType type, string json, CancellationToken ct = default)
        {
            var items = new List<object>();
            switch (type)
            {
                case IndexType.Management:
                    items.AddRange(JsonConvert.DeserializeObject<List<ManagementES>>(json));
                    break;
                case IndexType.Property:
                    items.AddRange(JsonConvert.DeserializeObject<List<PropertyES>>(json));
                    break;
            }
            
            var response = await _elasticClient.IndexManyAsync(items, type.ToString().ToLower(), ct);
            return ErrorHandler(response);
        }

        public async Task<bool> DeleteIndex(IndexType type, CancellationToken ct = default)
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
