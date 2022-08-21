using Faith.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Faith.Core.Models;

namespace Faith.Infrastructure.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(FaithPlatformContext context) : base(context) { }

        public async Task<Comment?> GetCommentById(int id)
        {
            return await _dbSet.Include(c => c.Mentor)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}