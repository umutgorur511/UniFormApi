using Microsoft.AspNetCore.Mvc;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller{
        private readonly ITokenRepository _tokenRepository;
        private readonly DataContext context;

        public TokenController(ITokenRepository tokenRepository, DataContext context) {
            _tokenRepository = tokenRepository;
            this.context = context;
        }
        
        [HttpPost("ValidateToken")]
        public async Task<ActionResult<ApiResponse<bool>>> ValidateToken(string accessToken)
        {
            return Ok(await _tokenRepository.ValidateTokenAsync(accessToken));
        }
    }
}
