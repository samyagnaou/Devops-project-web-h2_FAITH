namespace Faith.Core.Interfaces
{
    public interface ICommentService
    {
        Task<bool> AddCommentToPost(string userId, int messageId, string text);
    }
}