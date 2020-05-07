using Nest;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
