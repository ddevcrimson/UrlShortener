using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Workers
{
    public class ExpiredLinksWorker : IHostedService
    {
        private IServiceScope _scope;
        private ILogger<ExpiredLinksWorker> _logger;
        private CancellationTokenSource _source;
        private Task _task;

        public ExpiredLinksWorker(IServiceProvider provider, ILogger<ExpiredLinksWorker> logger)
        {
            _scope = provider.CreateScope();
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            _source = new CancellationTokenSource();
            _task = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await WaitNext(_source.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        _logger.LogInformation("Idling interrupted");
                        return;
                    }
                    _logger.LogInformation("Cleaning up old links");
                    await CleanupLinks();
                }
            }, _source.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            _source.Cancel();
            return Task.CompletedTask;
        }

        private async Task WaitNext(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var newDay = DateTime.UtcNow.AddDays(1).Date;
            await Task.Delay((int)(newDay - now).TotalMilliseconds, cancellationToken);
        }

        private Task CleanupLinks()
        {
            using var db = _scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            return db.Links
                .Where(_ => _.CreatedAt.AddDays(1) < DateTime.UtcNow)
                .ExecuteDeleteAsync();
        }
    }
}
