using ES.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ES.Application.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> SearchAsync(string query, string market, string state);
    }
}
