using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public string Href { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public string HexGuid() => Id.ToString("N");
    }
}
