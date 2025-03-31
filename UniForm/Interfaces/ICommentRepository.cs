using UniForm.Models;

namespace UniForm.Interfaces
{
    public interface ICommentRepository {
        Task<List<Comment>> GetCommentsByPostId(int postId);

        Task<Comment?> SetComment(Comment comment);
    }
}
