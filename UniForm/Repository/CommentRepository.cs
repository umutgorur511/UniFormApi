using Microsoft.EntityFrameworkCore;
using UniForm.Data;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Repository {
    public class CommentRepository : ICommentRepository {

        public readonly DataContext _context;

        public CommentRepository(DataContext context) {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByPostId(int postId) {
            return await _context.Comments
                .Where(x => x.PostId == postId)
                .ToListAsync();
        }

        public async Task<Comment?> SetComment(Comment comment) {
            var commentRow = new Comment {
                PostId = comment.PostId,
                UserId = comment.UserId,
                UserComment = comment.UserComment,
                CreateDate = DateTime.UtcNow,
                RecordStatus = 'A'
            };

            await _context.Comments.AddAsync(commentRow);
            await _context.SaveChangesAsync();

            return commentRow;
        }
    }
}
