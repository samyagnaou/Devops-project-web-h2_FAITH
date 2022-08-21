namespace Faith.Core.Interfaces
{
    public interface ICommentService
    {
        Task<bool> AddCommentToPost(string userId, int messageId, string text);
        Task<bool> EditComment(string userId, int id, string text);
        Task<bool> DeleteComment(string userId, int id);
    }
}