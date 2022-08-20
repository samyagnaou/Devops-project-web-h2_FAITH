using Faith.Core.Models;

namespace Faith.Core.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<Message?> GetMessageById(int id);
        Task<IEnumerable<Message>> GetAllMessagesForAStudent(string studentUserId);
        Task<IEnumerable<Message>> GetAllMessagesInMentorGroup(string mentorUserId);
    }
}