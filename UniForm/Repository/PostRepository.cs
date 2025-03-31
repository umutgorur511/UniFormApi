using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Repository
{
    public class PostRepository : IPostRepository {
        public readonly DataContext _context;
        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPosts()
        {
            return await _context.Posts.OrderByDescending(x=> x.CreateDate).ToListAsync();
        }
        public async Task<List<Post>> GetPostByUserId(int userId) {
            return await _context.Posts.OrderByDescending(x=> x.Id == userId).ToListAsync();
        } 
        public async Task<List<Post>> GetPostById(Models.Action action) {
            return await _context.Posts.OrderByDescending(x=> x.Id == action.PostId).ToListAsync();
        }

        public async Task<Post?> SetPost(Post post)
        {
            var postRow = new Post
            {
                UserId = post.UserId,
                Title = post.Title,
                Content = post.Content,
                UpdateDate = DateTime.UtcNow,
                CreateDate = DateTime.UtcNow,
                RecordStatus = 'A'
            };

            await _context.Posts.AddAsync(postRow);
            await _context.SaveChangesAsync();

            return postRow;
        }

    }
}
