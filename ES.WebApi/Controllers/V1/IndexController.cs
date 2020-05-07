using ES.Domain.Enums;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ES.WebApi.Controllers.V1
{
    //[ApiVersion("1")]
    //[Route("api/v{version:apiVersion}/index")]
    [Route("api/index")]
    [ApiController]
    public class IndexController: ControllerBase
    {
        private readonly IIndexService _indexService;

        public IndexController(IIndexService indexService)
        {
            _indexService = indexService;
        }

        [HttpPost("create")]
        public async Task CreateIndex([FromQuery] IndexType type, CancellationToken ct)
        {
            await _indexService.CreateIndex(type, ct);
        }

        [HttpPost("upload-json")]
        public async Task IndexIndex([FromQuery] IndexType type, [FromBody] string json)
        {
            await _indexService.IndexDocuments(type, json);
        }

        [HttpPost("delete")]
        public async Task DeleteIndex([FromQuery] IndexType type)
        {
            await _indexService.DeleteIndex(type);
        }
    }
}
