using Faith.Core.Models;

namespace Faith.Core.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetAllMessagesForAStudent(string userId);
        Task<IEnumerable<Message>> GetAllMessagesInMentorGroup(string mentorUserId);
        Task<bool> PostAMessage(string userId, string text, string? imageUrl);
    }
}