using ES.Application.Services.Interfaces;
using ES.Domain.Entities;
using ES.Domain.Enums;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace ES.WebApi.Controllers.V1
{
    //[ApiVersion("1")]
    //[Route("api/v{version:apiVersion}/index")]
    [Route("api/index")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IIndexService _indexService;
        private readonly IManagementService _managementService;
        private readonly ISearchService _searchService;

        public SearchController(
            ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] string market = null,
            [FromQuery] string state = null,
            CancellationToken ct)
        {
            var result = await _searchService.SearchAsync(query, market, state, ct);
            return Ok(result);
        }
    }
}
