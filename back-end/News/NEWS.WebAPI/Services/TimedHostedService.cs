using Microsoft.Extensions.Caching.Memory;
using NEWS.Entities.Services;
using NEWS.Services.Services;

namespace NEWS.WebAPI.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private Timer _testTimer;

        public TimedHostedService(IServiceProvider serviceProvider, ILogger<TimedHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Startup Hosted Service is stared");

            using (var scope = _serviceProvider.CreateScope())
            {
                var userTokenService = scope.ServiceProvider.GetRequiredService<IUserTokenService>();
                userTokenService.CacheBlockedTokens();
            }

            _testTimer = new Timer(Test, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void Test(object state)
        {

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopped");
            _testTimer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _testTimer?.Dispose();
        }
    }
}
