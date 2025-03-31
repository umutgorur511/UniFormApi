using Microsoft.AspNetCore.Mvc;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller{
        private readonly IUserRepository _userRepository;
        private readonly DataContext context;

        public UserController(IUserRepository userRepository, DataContext context) {
            _userRepository = userRepository;
            this.context = context;
        }
        
        [HttpGet("GetUserById")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int userId)
        {
            return Ok(await _userRepository.GetUserById(userId));        
        }

        
        [HttpPost("SetUser")]
        public async Task<ActionResult<ApiResponse<User>>> SetUser(User user) {
            return Ok(await _userRepository.SetUser(user));
        }

        [HttpPost("GetUserForLogin")]
        public async Task<ActionResult<ApiResponse<UserDto?>>> GetUserForLogin(UserInfo userInfo)
        {
            return Ok(await _userRepository.GetUserForLogin(userInfo));
        }
    }
}
