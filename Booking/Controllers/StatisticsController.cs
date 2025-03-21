using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { message = "API is up and running!" });

        }
    }
}
