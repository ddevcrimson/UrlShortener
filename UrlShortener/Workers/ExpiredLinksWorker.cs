using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Workers
{
    public class ExpiredLinksWorker : IHostedService
    {
        private ExpiredLinksWorkerConfiguration _conf;
        private ILogger<ExpiredLinksWorker> _logger;
        private CancellationTokenSource? _source;
        private IServiceProvider _provider;

        public ExpiredLinksWorker(ILogger<ExpiredLinksWorker> logger, ExpiredLinksWorkerConfiguration conf, IServiceProvider provider)
        {
            _provider = provider;
            _logger = logger;
            _conf = conf;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            _source = new CancellationTokenSource();
            Task.Run(async () =>
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
                    using var scope = _provider.CreateScope();
                    await scope.ServiceProvider
                        .GetRequiredService<ILinksService>()
                        .RemoveExpired(_conf.LinkTTL);
                }
            }, _source.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            _source?.Cancel();
            return Task.CompletedTask;
        }

        private async Task WaitNext(CancellationToken cancellationToken) => await Task.Delay(
            (int)_conf.RunEvery.TotalMilliseconds,
            cancellationToken
        );
    }
}
