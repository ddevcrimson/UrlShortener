using AutoMapper;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using UrlShortener.Models;
using UrlShortener.Models.Dto;

namespace UrlShortener.Controllers
{
    [Route("")]
    [ApiController]
    public partial class LinksController : ControllerBase
    {
        private ApplicationContext _db;
        private IMapper _mapper;

        public LinksController(ApplicationContext ctx, IMapper mapper)
        {
            _db = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public string Root() => "Hey there!";

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (!IdRegex().IsMatch(id)) return BadRequest("ID is not valid");
            var link = _db.Links.AsEnumerable().FirstOrDefault(_ => _.Id.ToString("N") == id);
            return link != null ? Redirect(link.Href) : NotFound("Link was not found");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LinkDto linkDto)
        {
            if (ModelState.IsValid)
            {
                var action = await _db.Links.AddAsync(_mapper.Map<Link>(linkDto));
                await _db.SaveChangesAsync();
                return Ok(action.Entity.Id.ToString("N"));
            }
            else
            {
                return BadRequest("\"href\" is not valid url");
            }
        }

        [GeneratedRegex("[a-z0-9]{32}")]
        private static partial Regex IdRegex();
    }
}
