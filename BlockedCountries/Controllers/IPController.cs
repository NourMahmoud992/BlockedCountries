using BlockedCountries.Helpers;
using BlockedCountries.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlockedCountries.Controllers
{
	[ApiController]
	[Route("api/ip")]
	public class IPController : ControllerBase
	{
		private readonly IpGeolocationService geoService;
		private readonly BlockedCountryService blockedCountryService;
		private readonly BlockedAttempService blockedAttempService;

		public IPController(IpGeolocationService geoService, BlockedCountryService blockedCountryService, BlockedAttempService blockedAttempService)
		{
			this.geoService = geoService;
			this.blockedCountryService = blockedCountryService;
			this.blockedAttempService = blockedAttempService;
		}
		/// <summary>
		/// This Method is used to get a country information using an ipAddress
		/// </summary>
		/// <param name="ipAddress">The Ip address needed to get the country info.</param>
		/// <returns>Returns a JSON string with the country's information</returns>
		[HttpGet("lookup")]
		public async Task<IActionResult> GetCountryByIp([FromQuery] string? ipAddress = null)
		{
			try
			{

				ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();

				var ipValidator = new IpValidator();

				if (string.IsNullOrEmpty(ipAddress) || !ipValidator.IsValidIp(ipAddress))
					return BadRequest(new { message = "Invalid IP address." });


				var countryCode = await geoService.GetCountryByIpAsync(ipAddress);
				if (countryCode == null)
					return BadRequest(new { message = "Failed to retrieve country information." });

				return Ok(new { ipAddress, countryCode });

			}
			catch (Exception ex)
			{
				blockedCountryService.LogAction(ex.Message);
				return BadRequest(ex.Message);
			}
		}
		/// <summary>
		/// this action takes the ip address automatically from the request and checks if the country is blocked or not
		/// </summary>
		/// <returns>true if blocked, false if not blocked</returns>
		[HttpGet("check-block")]
		public async Task<IActionResult> CheckBlocked()
		{
			try
			{
				var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

				var ipValidator = new IpValidator();
				bool isBlocked = false;
				if (string.IsNullOrEmpty(ipAddress) || !ipValidator.IsValidIp(ipAddress))
					return BadRequest(new { message = "Invalid IP address." });
				var countryCode = await geoService.GetCountryByIpAsync(ipAddress);
				HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent);
				if(string.IsNullOrEmpty( userAgent.ToString()))
				{
					return BadRequest(new { message = "Failed to retrieve user agent." });
				}
				if (countryCode == null)
					return BadRequest(new { message = "Failed to retrieve country information." });

				if (blockedCountryService.IsBlocked(countryCode.Country_Code2) || blockedCountryService.IsBlocked(countryCode.Country_Code3))
				{
					isBlocked = true;
				}
				blockedAttempService.LogAttempt(ipAddress, countryCode.Country_Code2, userAgent.ToString(), isBlocked);
				return new JsonResult(new { blocked = isBlocked });

				
			}
			catch (Exception ex)
			{
				blockedCountryService.LogAction(ex.Message);
				return BadRequest(ex.Message);
			}
		}
		
	}
}
