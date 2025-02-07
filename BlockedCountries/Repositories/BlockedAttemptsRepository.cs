using BlockedCountries.Models;
using System.Collections.Concurrent;

namespace BlockedCountries.Repositories
{
	public interface IBlockedAttemptsRepository
	{
		void LogAttempt(BlockedAttempts attempt);
		List<BlockedAttempts> GetBlockedAttempts(int page, int pageSize);
	}
	public class BlockedAttemptsRepository : IBlockedAttemptsRepository
	{
		private static readonly ConcurrentQueue<BlockedAttempts> _logs = new();
		public void LogAttempt(BlockedAttempts attempt)
		{
			_logs.Enqueue(attempt);
		}
		public List<BlockedAttempts> GetBlockedAttempts(int page, int pageSize)
		{
			var query = _logs.AsQueryable();
			return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
		}
	}
}
