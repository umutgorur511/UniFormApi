using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UniForm.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;
using UniForm.Models;

namespace UniForm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;

        public EmailController(EmailService emailService, IMemoryCache cache, IUserRepository userRepository)
        {
            _emailService = emailService;
            _cache = cache;
            _userRepository = userRepository;
        }

        [HttpPost("SendResetCode")]
        public async Task<IActionResult> SendResetCode(string email)
        {
            var resetCode = new Random().Next(100000, 1000000).ToString();
            _cache.Set(email, resetCode, TimeSpan.FromMinutes(5));

            var subject = "Şifre Sıfırlama Kodu";
            var body = $"Şifre sıfırlama kodunuz: {resetCode}";

            return Ok(await _emailService.SendEmailAsync(email, subject, body));
        }

        [HttpPost("VerifyCode")]
        public IActionResult VerifyCode([FromQuery] string email, [FromQuery] string code)
        {
            if (_cache.TryGetValue(email, out string storedCode) && storedCode == code) {
                var resetToken = GenerateResetToken();
                _cache.Set($"resetToken_{email}", resetToken, TimeSpan.FromMinutes(10));
                return Ok(new { success = true, resetToken });
            }
            return BadRequest(new { success = false, message = "Kod yanlış veya süresi dolmuş." });
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] string email, [FromQuery] string resetToken, [FromQuery] string newPassword)
        {
            if (_cache.TryGetValue($"resetToken_{email}", out string storedToken) && storedToken == resetToken)
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user == null) {
                    return BadRequest(new { success = false, message = "Kullanıcı bulunamadı." });
                }

                User newUser = new User {
                    Id = user.Data.Id,
                    Password = newPassword
                };

                await _userRepository.UpdatePassByUser(newUser);
                _cache.Remove($"resetToken_{email}");

                return Ok(new { success = true, message = "Şifre başarıyla değiştirildi." });
            }
            return BadRequest(new { success = false, message = "Geçersiz veya süresi dolmuş token." });
        }

        private string GenerateResetToken() {
            using (var rng = new RNGCryptoServiceProvider()) {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }
    }
}