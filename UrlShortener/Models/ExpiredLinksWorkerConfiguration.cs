namespace UrlShortener.Models
{
    public class ExpiredLinksWorkerConfiguration
    {
        public TimeSpan LinkTTL { get; set; }

        public TimeSpan RunEvery { get; set; }
    }
}
