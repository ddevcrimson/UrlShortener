using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Dto
{
    public class LinkDto
    {
        [Url]
        public string Href { get; set; }
    }
}
