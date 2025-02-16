using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegisterAPII.Models;

namespace RegisterAPII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EmailService _emailService;

        public UserController()
        {
            _emailService = new EmailService();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("Invalid user data.");

            string subject = "New User Issue Reported";
            string body = $@"
            <h3>User Issue Report Details:</h3>
            <p><strong>First Name:</strong> {user.FirstName}</p>
            <p><strong>Last Name:</strong> {user.LastName}</p>
            <p><strong>Email:</strong> {user.Email}</p>
            <p><strong>Mobile:</strong> {user.MobileNumber}</p>
            <p><strong>Issue:</strong> {user.Issue}</p>";

            try
            {
                await _emailService.SendEmailAsync("mwmn13554@gmail.com", subject, body);
                return Ok(new { message = "User issue reported and email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}