using Nest;
using System.Collections.Generic;
using System.Linq;

namespace ES.Infrastructure.ElasticSearch.Extensions
{
    public static class TermExtensions
    {
        public static ICollection<QueryContainer> AddTerm<T>(this ICollection<QueryContainer> queryContainers,
            string field,
            T term,
            double? boost = null)
        {
            queryContainers.Add(new TermQuery
            {
                Field = field,
                Value = term,
                Boost = boost
            });

            return queryContainers;
        }

        public static ICollection<QueryContainer> AddTerms<T>(this ICollection<QueryContainer> queryContainers,
            string field,
            ICollection<T> terms,
            double? boost = null)
        {
            if (terms == null || terms.Count == 0)
            {
                return queryContainers;
            }

            queryContainers.Add(new TermsQuery
            {
                Field = field,
                Terms = terms.Select(id => (object)id),
                Boost = boost
            });

            return queryContainers;
        }
    }
}
