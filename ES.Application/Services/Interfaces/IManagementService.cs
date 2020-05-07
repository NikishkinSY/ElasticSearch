using ES.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ES.Infrastructure.ElasticSearch.Interfaces
{
    public interface IManagementService
    {
        Task<IEnumerable<Management>> SearchAsync(string query, string market, string state);
    }
}
