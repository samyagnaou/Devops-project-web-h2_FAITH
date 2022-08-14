using Faith.Core.Models;

namespace Faith.Core.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetAllMessages(string userId);
    Task<bool> PostMessage(string userId, string text, string? imageUrl);
}