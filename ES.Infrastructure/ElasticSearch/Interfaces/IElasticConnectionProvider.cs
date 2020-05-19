using Elasticsearch.Net;
using Nest;

namespace ES.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticConnectionProvider
    {
        ConnectionSettings CreateConnectionSettings();
        ConnectionConfiguration CreateConnectionConfiguration();
    }
}
