using Faith.Core.Interfaces;
using Faith.Core.Models;

namespace Faith.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IStudentRepository _studentRepository;

        public MessageService(
            IMessageRepository messageRepository,
            IStudentRepository studentRepository)
        {
            _messageRepository = messageRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesForAStudent(string studentUserId)
            => await _messageRepository.GetAllMessagesForAStudent(studentUserId);

        public async Task<IEnumerable<Message>> GetAllMessagesInMentorGroup(string mentorUserId)
            => await _messageRepository.GetAllMessagesInMentorGroup(mentorUserId);

        public async Task<bool> PostAMessage(string userId, string text, string? imageUrl)
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
}