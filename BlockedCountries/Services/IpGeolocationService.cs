using System.Net;
using System.Net.Http.Json;
using BlockedCountries.Helpers;
using BlockedCountries.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class IpGeolocationService
{
	private readonly HttpClient _httpClient;
	private readonly string _apiKey;

	public IpGeolocationService(HttpClient httpClient, IOptions<IpGeolocationSettings> options)
	{
		_httpClient = httpClient;
		_apiKey = options.Value.ApiKey;
	}

	public async Task<IpGeolocationResponse> GetCountryByIpAsync(string ipAddress)
	{
		string url = $"https://api.ipgeolocation.io/ipgeo?apiKey={_apiKey}&ip={ipAddress}";
		
		//if (string.IsNullOrEmpty(ipAddress)) //this is used to get the current user ip address without the http context
		//{
		//	ipAddress = "auto"; 
		//}

		var response = await _httpClient.GetAsync(url);

		if (response.IsSuccessStatusCode)
		{
			// Deserialize the JSON response into the IpGeolocationResponse object
			var jsonResponse = await response.Content.ReadAsStringAsync();
			var geolocationData = JsonConvert.DeserializeObject<IpGeolocationResponse>(jsonResponse);
			if(geolocationData == null)
			{
				throw new Exception("Failed to deserialize geolocation data");
			}
			return geolocationData;
		}
		else
		{
			var errorMessage = await response.Content.ReadAsStringAsync();
			throw new Exception($"Error fetching geolocation data: {response.StatusCode}, {errorMessage}");
		}
		
	}

	
}