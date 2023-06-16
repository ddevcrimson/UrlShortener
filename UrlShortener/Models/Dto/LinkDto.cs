using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Dto
{
    public class LinkDto
    {
        [Required]
        [Url]
        public string Href { get; set; }
    }
}
