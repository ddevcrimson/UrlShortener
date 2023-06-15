using AutoMapper;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using UrlShortener.Models;
using UrlShortener.Models.Dto;
using UrlShortener.Services;
using UrlShortener.Utils;

namespace UrlShortener.Controllers
{
    [Route("")]
    [ApiController]
    public partial class LinksController : ControllerBase
    {
        private ApplicationContext _db;
        private IMapper _mapper;
        private ILinksService _linksService;

        public LinksController(ApplicationContext ctx, IMapper mapper, ILinksService service)
        {
            _db = ctx;
            _mapper = mapper;
            _linksService = service;
        }

        [HttpGet]
        public string Root() => "Hey there!";

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (!id.IsValidHexGuid()) return BadRequest("ID is not valid");
            var link = await _linksService.FirstOrNull(id);
            return link != null ? Redirect(link.Href) : NotFound("ID was not found");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LinkDto linkDto)
        {
            if (ModelState.IsValid)
            {
                var action = await _linksService.Insert(linkDto);
                return Ok(action.HexGuid());
            }
            else
            {
                return BadRequest("\"href\" is not valid url");
            }
        }
    }
}
