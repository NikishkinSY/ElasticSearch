using ES.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ES.WebApi.Controllers.V1
{
    //[ApiVersion("1")]
    //[Route("api/v{version:apiVersion}/index")]
    [Authorize]
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] ICollection<string> markets = default,
            [FromQuery] ICollection<string> states = default,
            [FromQuery] int size = 25)
        {
            var result = await _searchService.SearchAsync(query, markets, states, size, HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
