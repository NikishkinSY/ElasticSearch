using Nest;
using System.Collections.Generic;

namespace ES.Infrastructure.ElasticSearch
{
    public class SearchRequestBuilder<T>
    {
        private SearchRequest<T> _searchRequest;

        public SearchRequestBuilder<T> Build()
        {
            this._searchRequest = new SearchRequest<T>();

            return this;
        }

        public SearchRequestBuilder<T> Build(Indices indices)
        {
            this._searchRequest = new SearchRequest<T>(indices);

            return this;
        }

        public SearchRequest<T> GetRequest()
        {
            return _searchRequest;
        }

        public SearchRequestBuilder<T> SetFrom(int from)
        {
            _searchRequest.From = from;

            return this;
        }

        public SearchRequestBuilder<T> SetSize(int size)
        {
            _searchRequest.Size = size;

            return this;
        }

        public SearchRequestBuilder<T> SetExplain(bool explain = true)
        {
            _searchRequest.Explain = explain;

            return this;
        }

        public SearchRequestBuilder<T> SetCache(bool cache = true)
        {
            _searchRequest.RequestCache = cache;

            return this;
        }

        public SearchRequestBuilder<T> SetSourceExcludes(params string[] excludes)
        {
            _searchRequest.Source = new SourceFilter
            {
                Excludes = excludes
            };

            return this;
        }

        public SearchRequestBuilder<T> SetSourceIncludes(params string[] includes)
        {
            _searchRequest.Source = new SourceFilter
            {
                Includes = includes
            };

            return this;
        }

        public SearchRequestBuilder<T> SetAggregations(AggregationDictionary aggregationDictionary)
        {
            _searchRequest.Aggregations = aggregationDictionary;

            return this;
        }

        public SearchRequestBuilder<T> SetQuery(QueryContainer queryContainer)
        {
            _searchRequest.Query = queryContainer;

            return this;
        }

        public SearchRequestBuilder<T> SetRescore(IList<IRescore> rescore)
        {
            _searchRequest.Rescore = rescore;

            return this;
        }

        public SearchRequestBuilder<T> SetPreference(string preference)
        {
            if (!string.IsNullOrWhiteSpace(preference))
            {
                _searchRequest.Preference = preference;
            }

            return this;
        }
    }
}
