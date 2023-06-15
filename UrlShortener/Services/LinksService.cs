using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using UrlShortener.Models.Dto;

namespace UrlShortener.Services
{
    public class LinksService : ILinksService
    {
        private ApplicationContext _context;
        private IMapper _mapper;

        public LinksService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Link?> FirstOrNull(string guid)
        {
            await foreach (var i in _context.Links.AsAsyncEnumerable())
            {
                if (i.HexGuid() == guid)
                {
                    return i;
                }
            }
            return null;
        }

        public async Task<Link> Insert(LinkDto dto)
        {
            var entity = await _context.Links.AddAsync(_mapper.Map<Link>(dto));
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public Task<int> RemoveExpired(TimeSpan deadline)
        {
            var now = DateTime.UtcNow;
            return _context.Links
                .Where(i => (i.CreatedAt + deadline) < now)
                .ExecuteDeleteAsync();
        }
    }
}
