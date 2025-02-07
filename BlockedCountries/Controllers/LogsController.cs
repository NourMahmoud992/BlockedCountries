using BlockedCountries.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
	[ApiController]
	[Route("api/logs")]
	public class LogsController : ControllerBase
	{
		private readonly BlockedAttempService blockedAttempService;

		public LogsController(BlockedAttempService blockedAttempService)
		{
			this.blockedAttempService = blockedAttempService;
		}
		/// <summary>
		/// This Method is used to return a list of the check attempts.
		/// </summary>
		/// <param name="page">The Page Needed to be returned</param>
		/// <param name="pageSize">The page size</param>
		/// <returns></returns>
		[HttpGet("blocked-attempts")]
		public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var attempts = blockedAttempService.GetBlockedAttempts(page, pageSize);
			return Ok(new { page, pageSize, results = attempts });
		}

	}
}
