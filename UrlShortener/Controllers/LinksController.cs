using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models.Dto;
using UrlShortener.Services;
using UrlShortener.Utils;

namespace UrlShortener.Controllers
{
    [Route("")]
    [ApiController]
    public partial class LinksController : ControllerBase
    {
        private ILinksService _service;

        public LinksController(ILinksService service)
        {
            _service = service;
        }

        [HttpGet]
        public string Hello() => "Hey there!";

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (!id.IsValidHexGuid()) return BadRequest("ID is not valid");
            var link = await _service.FirstOrNull(id);
            return link != null ? Redirect(link.Href) : NotFound("ID was not found");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LinkDto linkDto)
        {
            if (ModelState.IsValid)
            {
                var action = await _service.Insert(linkDto);
                return Ok(action.HexGuid());
            }
            else
            {
                return BadRequest("\"href\" is not valid url");
            }
        }
    }
}
