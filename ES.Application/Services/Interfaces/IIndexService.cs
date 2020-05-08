using ES.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Application.Services.Interfaces
{
    public interface IIndexService
    {
        Task<bool> CreateIndex(IndexType type, CancellationToken ct);
        Task<bool> IndexDocuments(IndexType type, string json, CancellationToken ct);
        Task<bool> DeleteIndex(IndexType type, CancellationToken ct);
    }
}
