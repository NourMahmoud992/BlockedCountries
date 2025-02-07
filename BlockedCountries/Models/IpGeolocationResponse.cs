namespace BlockedCountries.Models
{
	public class IpGeolocationResponse
	{
		public string Ip { get; set; } = default!;
		public string Continent_Code { get; set; } = default!;
		public string Continent_Name { get; set; } = default!;
		public string Country_Code2 { get; set; } = default!;
		public string Country_Code3 { get; set; } = default!;
		public string Country_Name { get; set; } = default!;
		public string CountryNameOfficial { get; set; } = default!;
		public string CountryCapital { get; set; } = default!;
		public string StateProv { get; set; } = default!;
		public string StateCode { get; set; } = default!;
		public string District { get; set; } = default!;
		public string City { get; set; } = default!;
		public string Zipcode { get; set; } = default!;
		public string Latitude { get; set; } = default!;
		public string Longitude { get; set; } = default!;
		public bool IsEu { get; set; }
		public string CallingCode { get; set; } = default!;
		public string CountryTld { get; set; } = default!;
		public string Languages { get; set; } = default!;
		public string CountryFlag { get; set; } = default!;
		public string GeonameId { get; set; } = default!;
		public string Isp { get; set; } = default!;
		public string Organization { get; set; } = default!;
		public string CountryEmoji { get; set; } = default!;
		public Currency Currency { get; set; } = default!;
		public TimeZone Time_zone { get; set; } = default!;
	}

}
