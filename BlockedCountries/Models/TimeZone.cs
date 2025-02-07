namespace BlockedCountries.Models
{
	public class TimeZone
	{
		public string Name { get; set; } = default!;
		public double Offset { get; set; }
		public double Offset_with_dst { get; set; }
		public string Current_time { get; set; } = default!;
		public long Current_time_unix { get; set; }
		public bool Is_dst { get; set; }
		public int Dst_savings { get; set; }
		public bool Dst_exists { get; set; }
	}
}
