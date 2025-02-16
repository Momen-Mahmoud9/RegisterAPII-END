using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegisterAPII.Models;
using System;
using System.Threading.Tasks;

namespace RegisterAPII.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EventController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveCard([FromBody] PurchaseRequest request)
        {
            // التحقق من أن الطلب غير فارغ وأن البيانات المطلوبة موجودة
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Category))
            {
                return BadRequest(new { message = "Invalid request: Email and Category are required." });
            }

            // التحقق من صحة البريد الإلكتروني
            if (!IsValidEmail(request.Email))
            {
                return BadRequest(new { message = "Invalid email format." });
            }

            try
            {
                // إرسال بريد التأكيد
                string subject = "Virtual Card Reservation - Event Hub";
                string body = $"<h2>Reservation Confirmed!</h2><p>You have successfully reserved a virtual card for: <strong>{request.Category}</strong></p>";

                await _emailService.SendEmailAsync(request.Email, subject, body);

                // إرسال رد ناجح للفرونت إند
                return Ok(new
                {
                    message = "Virtual card reserved successfully!",
                    details = new
                    {
                        email = request.Email,
                        category = request.Category,
                        expirationDate = "31 December 2025"
                    }
                });
            }
            catch (Exception ex)
            {
                // إرسال رسالة خطأ في حالة فشل العملية
                return StatusCode(500, new { message = "Error processing your request.", error = ex.Message });
            }
        }

        // دالة للتحقق من صحة البريد الإلكتروني
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}