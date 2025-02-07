using System.Diagnostics.Metrics;

namespace BlockedCountries.Models
{
	public class BlockedAttempts
	{
		public string IP_Address { get; set; } = default!;
		public DateTime Timestamp { get; set; } = default!;
		public string Country_Code { get; set; } = default!;
		public bool IsBlocked { get; set; } = default!;
		public string UserAgent { get; set; } = default!;
	}
}
