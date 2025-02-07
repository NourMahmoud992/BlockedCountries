using System;
using System.Net;

namespace BlockedCountries.Helpers
{
	public class IpValidator
	{
		public bool IsValidIp(string ipAddress)
		{
			// Try parsing the IP address
			return IPAddress.TryParse(ipAddress, out _);
		}
	}

}
