using UrlShortener.Models;
using UrlShortener.Models.Dto;

namespace UrlShortener.Services
{
    public interface ILinksService
    {
        Task<Link?> FirstOrNull(string uuid);

        Task<Link> Insert(LinkDto dto);

        Task<int> RemoveExpired(TimeSpan deadline);
    }
}
