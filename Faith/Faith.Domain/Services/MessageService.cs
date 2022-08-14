using Faith.Core.Interfaces;
using Faith.Core.Models;

namespace Faith.Core.Services;

public class MessageService : IMessageService
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IStudentRepository _studentRepository;

    public MessageService(IRepository<Message> messageRepository,
        IStudentRepository studentRepository)
    {
        _messageRepository = messageRepository;
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<Message>> GetAllMessages(string userId)
    {
        var student = await _studentRepository.GetByUserId(userId);
        if (student != null)
        {
            return student.Messages;
        }
        return Enumerable.Empty<Message>();
    }

    public async Task<bool> PostMessage(string userId, string text, string? imageUrl)
    {
        var student = await _studentRepository.GetByUserId(userId);
        if (student == null)
            return false;

        var message = new Message
        {
            Text = text,
            ImageUrl = imageUrl,
            StudentId = student.Id,
            CreatedAt = DateTime.Now,
        };
        try
        {
            await _messageRepository.AddAsync(message);
            return true;
        }
        catch (Exception) { return false; }
    }
}