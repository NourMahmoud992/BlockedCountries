namespace BlockedCountries.Models
{
	public class TemporaryBlockedCountry
	{
		public string CountryCode { get; set; } = default!;
		public DateTime ExpiryTime { get; set; }
	}
}
