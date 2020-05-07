using ES.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ES.WebApi.Controllers.V1
{
    //[ApiVersion("1")]
    //[Route("api/v{version:apiVersion}/index")]
    [Authorize]
    [Route("api/index")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] string market = null,
            [FromQuery] string state = null)
        {
            var result = await _searchService.SearchAsync(query, market, state, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
