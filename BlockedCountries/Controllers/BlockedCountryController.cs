using BlockedCountries.Helpers;
using BlockedCountries.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/countries")]
public class BlockedCountriesController : ControllerBase
{
	private readonly BlockedCountryService blockedCountryService;
	private readonly IpGeolocationSettings options;

	public BlockedCountriesController(BlockedCountryService blockedCountryService, IOptions<IpGeolocationSettings> options)
	{
		this.blockedCountryService = blockedCountryService;
		this.options = options.Value;
	}
	/// <summary>
	/// Use This Method to Block a Country
	/// </summary>
	/// <param name="countryCode">This is the country code ex:"EG"</param>
	/// <returns>returns ok if the operation was successful and bad request if failed</returns>
	[HttpPost("block")]
	public IActionResult BlockCountry(string countryCode)
	{
		try
		{
			blockedCountryService.BlockCountry(countryCode);

			return Ok(new { message = $"{countryCode} - Country blocked." });
		}

		catch (Exception ex)
		{
			blockedCountryService.LogAction(ex.Message);
			return BadRequest(new { message = ex.Message });
		}
	}
	/// <summary>
	/// Use This Method to un-block a country
	/// </summary>
	/// <param name="countryCode">This is the country code ex:"EG"</param>
	/// <returns>returns ok if the operation was successful and bad request if failed</returns>
	[HttpDelete("block/{countryCode}")]
	public IActionResult UnblockCountry(string countryCode)
	{
		try
		{

			if (!blockedCountryService.IsBlocked(countryCode))
				return NotFound(new { message = "Country not found in blocked list." });

			blockedCountryService.UnblockCountry(countryCode);
			return Ok(new { message = "Country unblocked." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
	/// <summary>
	/// This Method is used to return a list of the blocked countries.
	/// </summary>
	/// <param name="page">The Page Needed to be returned</param>
	/// <param name="pageSize">The page size</param>
	/// <param name="filter"> filter to the country codes</param>
	/// <returns></returns>
	[HttpGet("blocked")]
	public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filter = null)
	{
		try
		{

			var countries = blockedCountryService.GetBlockedCountries(page, pageSize, filter);
			return Ok(new { page, pageSize, results = countries });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("temporal-block")]
	public async Task<IActionResult> TemporarilyBlockCountry([FromQuery] string CountryCode, int DurationMinutes )
	{
		try
		{

			// Validate duration
			if (DurationMinutes < 1 || DurationMinutes > 1440)
			{
				return BadRequest(new { message = "Duration must be between 1 and 1440 minutes." });
			}

			if (!IsValidCountryCode(CountryCode))
			{
				return BadRequest(new { message = "Invalid country code." });
			}

			// Check if already blocked
			if (!blockedCountryService.TryBlockCountry(CountryCode, DurationMinutes))
			{
				return Conflict(new { message = "Country is already temporarily blocked." });
			}

			return Ok(new { message = $"Country {CountryCode} is temporarily blocked for {DurationMinutes} minutes." });
		}
		catch(Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	private bool IsValidCountryCode(string countryCode)
	{
		return options.CountryCodes.Contains(countryCode);
	}

}
