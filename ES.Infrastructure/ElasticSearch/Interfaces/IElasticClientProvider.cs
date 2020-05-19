using Nest;

namespace ES.Infrastructure.ElasticSearch.Interfaces
{
    public interface IElasticClientProvider
    {
        IElasticClient Get();
    }
}
