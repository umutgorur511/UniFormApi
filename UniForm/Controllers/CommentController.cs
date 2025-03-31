using Microsoft.AspNetCore.Mvc;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller {
        private readonly ICommentRepository _commentRepository;
        private readonly DataContext context;

        public CommentController(ICommentRepository commentRepository, DataContext context) {
            _commentRepository= commentRepository;
            this.context = context;
        }


        [HttpGet("GetCommentsByPostId")]
        public async Task<ActionResult<Comment>> GetCommentsByPostId(int postId) {
            var comment = await _commentRepository.GetCommentsByPostId(postId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(comment);
        }


        [HttpPost("SetComment")]
        public async Task<ActionResult<User>> SetComment(Comment comment) {
            var users = await _commentRepository.SetComment(comment);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }
    }
}
