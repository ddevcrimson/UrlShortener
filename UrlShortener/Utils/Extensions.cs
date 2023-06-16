using UrlShortener.Models;
using UrlShortener.Workers;

namespace UrlShortener.Utils
{
    public static class Extensions
    {
        public static bool IsValidHexGuid(this string guid) =>
            guid.Length == 32 && guid.All(i => i is
                (>= '0' and <= '9') or
                (>= 'a' and <= 'z')
            );

        public static void AddExpiredLinksWorker(this IServiceCollection services, ExpiredLinksWorkerConfiguration conf)
        {
            services.AddSingleton(conf);
            services.AddHostedService<ExpiredLinksWorker>();
        }
    }
}
