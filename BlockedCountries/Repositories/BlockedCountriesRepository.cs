namespace BlockedCountries.Repositories
{
	using BlockedCountries.Models;
	using System.Collections.Concurrent;

	public interface IBlockedCountriesRepository
	{
		bool BlockCountry(string countryCode);
		bool UnblockCountry(string countryCode);
		bool IsBlocked(string countryCode);
		List<string> GetBlockedCountries(int page, int pageSize, string? filter);
		List<string> GetLogs();
		void LogAction(string message);
		bool TryBlockCountry(string countryCode, int durationMinutes);
		void RemoveExpiredBlocks();
	}

	public class BlockedCountriesRepository : IBlockedCountriesRepository
	{
		private readonly ConcurrentDictionary<string, bool> _blockedCountries = new();
		private static readonly ConcurrentQueue<string> _logs = new();
		private static readonly ConcurrentDictionary<string,DateTime> _tempBlockedCountries = new();

		public bool BlockCountry(string countryCode)
		{
			
			bool result = _blockedCountries.TryAdd(countryCode, true);
			if (!result)
			{
				throw new Exception("Failed to block country");
			}
			
			LogAction($"Blocked country: {countryCode}");
			
			return result;
		}

		public bool UnblockCountry(string countryCode)
		{
			
			bool result = _blockedCountries.TryRemove(countryCode, out _);
			if (!result)
			{
				throw new Exception("Failed to unblock country");
			}
			LogAction($"Unblocked country: {countryCode}");
			
			return result;
		}

		public bool IsBlocked(string countryCode) => _blockedCountries.ContainsKey(countryCode);

		public List<string> GetBlockedCountries(int page, int pageSize, string? filter)
		{

			var query = _blockedCountries.Keys.AsQueryable();
			if (!string.IsNullOrWhiteSpace(filter))
				query = query.Where(c => c.Contains(filter, StringComparison.OrdinalIgnoreCase));

			return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
		}

		// Get all logs
		public List<string> GetLogs()
		{
			return _logs.ToList();
		}

		// Log actions (only keeps the latest 100 logs)
		public void LogAction(string message)
		{
			if (_logs.Count >= 100)
				_logs.TryDequeue(out _); // Remove oldest log

			_logs.Enqueue($"{DateTime.UtcNow}: {message}");
		}
		public bool TryBlockCountry(string countryCode, int durationMinutes)
		{
			if (_tempBlockedCountries.ContainsKey(countryCode))
			{
				return false;
			}
			var expiryTime = DateTime.UtcNow.AddMinutes(durationMinutes);

			_tempBlockedCountries[countryCode] = expiryTime;
			return true;
		}

		public void RemoveExpiredBlocks()
		{
			var now = DateTime.UtcNow;
			foreach (var entry in _tempBlockedCountries)
			{
				if (entry.Value <= now)
				{
					_tempBlockedCountries.TryRemove(entry.Key, out _);
				}
			}
		}

		public bool IsCountryTemporaryBlocked(string countryCode)
		{
			return _tempBlockedCountries.ContainsKey(countryCode);
		}
	}
}
