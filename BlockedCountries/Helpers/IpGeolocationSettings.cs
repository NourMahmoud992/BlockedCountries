namespace BlockedCountries.Helpers
{
	public class IpGeolocationSettings
	{
		public string ApiKey { get; set; } = string.Empty;
		public List<string> CountryCodes { get; set; } = new List<string>();
	}
}
