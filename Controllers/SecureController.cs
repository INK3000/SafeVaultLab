using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("api/secure")]
    public class SecureController : ControllerBase
    {
        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok($"Welcome, {User.Identity?.Name}");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin-dashboard")]
        public IActionResult AdminDashboard()
        {
            return Ok("Welcome to the Admin Dashboard.");
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("user-area")]
        public IActionResult UserArea()
        {
            return Ok("Welcome to the User Area.");
        }
    }
}
