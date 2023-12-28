using Microsoft.Extensions.Caching.Memory;
using NEWS.Entities.Services;
using NEWS.Services.Services;

namespace NEWS.WebAPI.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IFileService _fileService;
        private Timer _cacheTokenTimer;
        private Timer deleteExpiredTokensTimer;
        private Timer deleteUnusedFilesTimer;

        public TimedHostedService(IServiceProvider serviceProvider, ILogger<TimedHostedService> logger,
            IFileService fileService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _fileService = fileService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Startup Hosted Service is stared");

            // Get blocked token from DB and save to static variable to cache data
            _cacheTokenTimer = new Timer(CacheBlockedTokensHosted, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            // Delete exprired tokens
            deleteExpiredTokensTimer = new Timer(DeleteExpiredTokensHosted, null, TimeSpan.FromMinutes(10), TimeSpan.FromDays(1));


            // Delete draft file when user composing Post, uploading image then deleting image on the text editor.
            deleteUnusedFilesTimer = new Timer(DeleteUnusedFilesHosted, null, TimeSpan.FromMinutes(15), TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void CacheBlockedTokensHosted(object state)
        {
            _logger.LogInformation("CacheBlockedTokensAsync is stared");
            using (var scope = _serviceProvider.CreateScope())
            {
                var userTokenService = scope.ServiceProvider.GetRequiredService<IUserTokenService>();
                userTokenService.CacheBlockedTokensAsync().Wait();
            }
        }
        
        private void DeleteExpiredTokensHosted(object state)
        {
            _logger.LogInformation("DeleteExpiredTokensHosted is stared");
            using (var scope = _serviceProvider.CreateScope())
            {
                var userTokenService = scope.ServiceProvider.GetRequiredService<IUserTokenService>();
                userTokenService.DeleteExpiredTokensAsync().Wait();
            }
        }
        
        private void DeleteUnusedFilesHosted(object state)
        {
            _logger.LogInformation("DeleteUnusedFilesHosted is stared");
            _fileService.DeletedUnusedFilesAsync().Wait();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopped");
            _cacheTokenTimer.Change(Timeout.Infinite, 0);
            deleteExpiredTokensTimer.Change(Timeout.Infinite, 0);
            deleteUnusedFilesTimer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cacheTokenTimer?.Dispose();
            deleteExpiredTokensTimer?.Dispose();
            deleteUnusedFilesTimer?.Dispose();
        }
    }
}
