using BlockedCountries.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

public class BlockedCountryService
{
	//private static readonly ConcurrentDictionary<string, bool> _blockedCountries = new();
	//private static readonly ConcurrentQueue<string> _logs = new();
	private readonly IBlockedCountriesRepository blockedCountriesRepository;

	public BlockedCountryService(IBlockedCountriesRepository blockedCountriesRepository)
	{
		this.blockedCountriesRepository = blockedCountriesRepository;
	}
	// Add a country to the blocked list
	public bool BlockCountry(string countryCode)
	{
	
		if(string.IsNullOrEmpty(countryCode))
		{
			throw new Exception("Country Code is Empty");
		}
		if(blockedCountriesRepository.IsBlocked(countryCode))
		{

			throw new Exception("Country is already blocked"); ;
		}
		var result = blockedCountriesRepository.BlockCountry(countryCode);
		if(result)
		{
			return true;
		}
		else
		{
			throw new Exception("Failed to block country");
		}
		
	}

	// Remove a country from the blocked list
	public void UnblockCountry(string countryCode)
	{
		if(!blockedCountriesRepository.IsBlocked(countryCode))
		{
			throw new Exception("Country is not blocked");
		}
		var result = blockedCountriesRepository.UnblockCountry(countryCode);
		if (!result)
		{
			throw new Exception($"Failed to unblock country: {countryCode}");
		}
		
	}

	// Check if a country is blocked
	public bool IsBlocked(string countryCode)
	{
		return blockedCountriesRepository.IsBlocked(countryCode);
	}

	// Get all blocked countries
	public List<string> GetBlockedCountries(int page, int pageSize, string? filter)
	{
		return blockedCountriesRepository.GetBlockedCountries(page,pageSize,filter);
	}
	public List<string> GetLogs()
	{
		return blockedCountriesRepository.GetLogs();
	}

	public void LogAction(string message)
	{
		blockedCountriesRepository.LogAction(message);
	}

	public bool TryBlockCountry(string countryCode, int durationTime)
	{
		return blockedCountriesRepository.TryBlockCountry(countryCode, durationTime);
	}
}
