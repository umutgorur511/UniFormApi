using UniForm.Entity;
using UniForm.Models;
using Action = UniForm.Models.Action;

namespace UniForm.Interfaces
{
    public interface IActionRepository {
        Task<ActionInfo> GetActionCountsByPostId(int postId);
        Task<List<Models.Action>> GetActionByUserId(Models.Action actions);
        Task<Models.Action?> SetAction(Models.Action action);
    }
}
