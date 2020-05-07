using Nest;
using System.Collections.Generic;

namespace ES.Infrastructure.ElasticSearch.Extensions
{
    public static class MatchExtensions
    {
        public static ICollection<QueryContainer> AddMatch(this ICollection<QueryContainer> containers,
            string field,
            string query,
            Operator? @operator = null,
            MinimumShouldMatch minimumShouldMatch = default,
            double? boost = null,
            bool autoGenerateSynonymsPhraseQuery = true)
        {
            containers.Add(new MatchQuery
            {
                Field = field,
                Query = query,
                Operator = @operator,
                MinimumShouldMatch = minimumShouldMatch,
                Boost = boost,
                AutoGenerateSynonymsPhraseQuery = autoGenerateSynonymsPhraseQuery,
                FuzzyTranspositions = false
            });

            return containers;
        }

        public static ICollection<QueryContainer> AddMatchPhrase(this ICollection<QueryContainer> containers,
            string field,
            string query,
            int? slop = null,
            double? boost = null)
        {
            containers.Add(new MatchPhraseQuery
            {
                Field = field,
                Query = query,
                Slop = slop,
                Boost = boost
            });

            return containers;
        }
    }
}
