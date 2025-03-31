using Microsoft.AspNetCore.Mvc;
using UniForm.Data;
using UniForm.Interfaces;
using UniForm.Models;
using UniForm.Repository;
using Action = UniForm.Models.Action;

namespace UniForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : Controller
    {
        private readonly IActionRepository _actionRepository;
        private readonly DataContext _context;

        public ActionController(IActionRepository actionRepository, DataContext context)
        {
            _actionRepository = actionRepository;
            _context = context;
        }

        [HttpGet("GetActionByPostId")]
        public async Task<ActionResult<int>> GetActionByPostId([FromQuery] Action action) {
            var actionsCount = await _actionRepository.GetActionByUserId(action);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(actionsCount);
        }


        [HttpGet("GetActionByUserId")]
        public async Task<ActionResult<Post>> GetActionByUserId(Action action)
        {
            var post = await _actionRepository.GetActionByUserId(action);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(post);
        }

        [HttpPost("SetAction")]
        public async Task<ActionResult<Post>> SetAction(Action action)
        {
            var actionRow = await _actionRepository.SetAction(action);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(actionRow);
        }
    }
}
