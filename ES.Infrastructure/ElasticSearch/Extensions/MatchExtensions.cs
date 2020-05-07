using Nest;
using System.Collections.Generic;

namespace ES.Infrastructure.ElasticSearch.Extensions
{
    public static class MatchExtensions
    {
        public static ICollection<QueryContainer> AddMatch(this ICollection<QueryContainer> containers,
            string field,
            string query,
            Operator? @operator = default,
            MinimumShouldMatch minimumShouldMatch = default,
            double? boost = default,
            bool? autoGenerateSynonymsPhraseQuery = default,
            bool? fuzzyTranspositions = default)
        {
            containers.Add(new MatchQuery
            {
                Field = field,
                Query = query,
                Operator = @operator,
                MinimumShouldMatch = minimumShouldMatch,
                Boost = boost,
                AutoGenerateSynonymsPhraseQuery = autoGenerateSynonymsPhraseQuery,
                FuzzyTranspositions = fuzzyTranspositions
            });

            return containers;
        }

        public static ICollection<QueryContainer> AddMatchPhrase(this ICollection<QueryContainer> containers,
            string field,
            string query,
            int? slop = default,
            double? boost = default)
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

        public static ICollection<QueryContainer> AddMultiMatch(this ICollection<QueryContainer> containers,
            string[] fields,
            string query,
            TextQueryType @type = TextQueryType.BestFields,
            Operator? @operator = default,
            MinimumShouldMatch minimumShouldMatch = default,
            double? boost = default,
            bool? autoGenerateSynonymsPhraseQuery = default,
            bool? fuzzyTranspositions = default)
        {
            containers.Add(new MultiMatchQuery
            {
                Fields = fields,
                Query = query,
                Operator = @operator,
                Type = @type,
                MinimumShouldMatch = minimumShouldMatch,
                AutoGenerateSynonymsPhraseQuery = autoGenerateSynonymsPhraseQuery,
                Boost = boost,
                FuzzyTranspositions = fuzzyTranspositions
            });

            return containers;
        }
    }
}
