using ES.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Application.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<BaseItem>> SearchAsync(string query, string market, string state, CancellationToken ct);
    }
}
