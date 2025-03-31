using UniForm.Models;

namespace UniForm.Interfaces
{
    public interface IPostRepository {
        Task<List<Post>> GetPosts();
        Task<Post?> SetPost(Post post);

        Task<List<Post>> GetPostByUserId(int userId);
    }
}
