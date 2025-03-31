using Microsoft.EntityFrameworkCore;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Repository
{
    public class ActionRepository : IActionRepository
    {
        public readonly DataContext _context;
        private readonly IPostRepository _postRepository;
        public ActionRepository(DataContext context ,IPostRepository postRepository) {
            _postRepository = postRepository;
            _context = context;
        }

        /// <summary>
        /// postun idsine göre beğeni ve kaydetme sayısını getirir
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<ActionInfo> GetActionCountsByPostId(int postId) {
               int likeCount = await _context.Actions
                .Where(x => x.PostId == postId && x.ActionType == Enum.ActionType.Like)
                .CountAsync();
            int markCount = await _context.Actions
                .Where(x => x.PostId == postId && x.ActionType == Enum.ActionType.BookMark)
                .CountAsync();
            ActionInfo actionInfo = new ActionInfo {
                PostId = postId,
                LikeCount = likeCount,
                MarkCount = markCount
            };
            return actionInfo;
        }

        /// <summary>
        /// useridsi ve action type ile post idlarını döner
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<List<Models.Action>> GetActionByUserId(Models.Action action) {
            return await _context.Actions
                .Where(x => x.UserId == action.UserId && x.ActionType == action.ActionType).ToListAsync();
        }

        /// <summary>
        /// Action insert
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<Models.Action?> SetAction(Models.Action action)
        {
            var actionRow = new Models.Action
            {
                PostId = action.PostId,
                UserId = action.UserId,
                ActionType = action.ActionType,
                ActionDate = DateTime.UtcNow,
                RecordStatus = 'A'
            };

            await _context.Actions.AddAsync(actionRow);
            await _context.SaveChangesAsync();

            return actionRow;
        }
    }
}
