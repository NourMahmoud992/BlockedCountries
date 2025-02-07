using BlockedCountries.Repositories;

namespace BlockedCountries.Services
{
	public class TemporaryBlockCleanupService : BackgroundService
	{
		private readonly IBlockedCountriesRepository blockedCountriesRepository;
		private readonly TimeSpan cleanupInterval = TimeSpan.FromMinutes(5);

		public TemporaryBlockCleanupService( IBlockedCountriesRepository blockedCountriesRepository)
		{
			this.blockedCountriesRepository = blockedCountriesRepository;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				blockedCountriesRepository.RemoveExpiredBlocks();
				await Task.Delay(cleanupInterval, stoppingToken);
			}
		}
	}
}
