using Elasticsearch.Net;

namespace ES.Infrastructure.AWS.Interfaces
{
    public interface IAWSConnectionProvider
    {
        IConnection Create();
    }
}
