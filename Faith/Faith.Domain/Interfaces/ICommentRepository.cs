using Faith.Core.Models;

namespace Faith.Core.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment?> GetCommentById(int id);
    }
}