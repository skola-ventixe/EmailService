using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;

        [HttpPost("verify")]
        public async Task<IActionResult> SendVerificationEmail([FromBody] SendVerificationCodeDto request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _emailService.SendVerificationEmailAsync(request);
            if(!response.Succeeded)
                return BadRequest(response.Error);

            return Ok(response.Message);
        }
    }
}
