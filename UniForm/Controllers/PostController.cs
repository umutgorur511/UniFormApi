using Microsoft.AspNetCore.Mvc;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly DataContext _context;

        public PostController(IPostRepository postRepository, DataContext context)
        {
            _postRepository = postRepository;
            _context = context;
        }

        [HttpGet("GetPosts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _postRepository.GetPosts();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost("SetPost")]
        public async Task<ActionResult<Post>> SetPost(Post post)
        {
            var createdPost = await _postRepository.SetPost(post);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(createdPost);
        }
    }

}
