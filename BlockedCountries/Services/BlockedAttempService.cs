using BlockedCountries.Models;
using BlockedCountries.Repositories;
using System.Net.Http;

namespace BlockedCountries.Services
{
	public class BlockedAttempService
	{
		private readonly IBlockedAttemptsRepository blockedAttemptsRepository;
		private readonly HttpClient httpClient;

		public BlockedAttempService(IBlockedAttemptsRepository blockedAttemptsRepository, HttpClient httpClient)
		{
			this.blockedAttemptsRepository = blockedAttemptsRepository;
			this.httpClient = httpClient;
		}
		public void LogAttempt(string ipAddress, string countryCode,string userAgent, bool isBlocked)
		{
			var attempt = new BlockedAttempts
			{
				IP_Address = ipAddress,
				Country_Code = countryCode,
				Timestamp = DateTime.UtcNow,
				UserAgent = userAgent,
				IsBlocked = isBlocked
			};
			 blockedAttemptsRepository.LogAttempt(attempt);
		}
		public List<BlockedAttempts> GetBlockedAttempts(int page, int pageSize) {
			
			return blockedAttemptsRepository.GetBlockedAttempts(page, pageSize);
		}
	}
}
