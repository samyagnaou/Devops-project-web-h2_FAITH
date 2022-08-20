using Faith.Core.Interfaces;
using Faith.Core.Models;

namespace Faith.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRepository<Comment> _commentRepository;

        public CommentService(
            IMessageRepository messageRepository,
            IRepository<Comment> commentRepository)
        {
            _messageRepository = messageRepository;
            _commentRepository = commentRepository;
        }

        public async Task<bool> AddCommentToPost(string userId, int messageId, string text)
        {
            var message = await _messageRepository.GetMessageById(messageId);
            if (message == null)
                return false;

            var comment = new Comment
            {
                MessageId = messageId,
                Text = text,
            };

            if (message.Student.MemberId == userId)
                comment.Student = message.Student;
            else if (message.Student.Mentors.Any(m => m.MemberId == userId))
                comment.Mentor = message.Student.Mentors.First(m => m.MemberId == userId);
            else
                return false;

            try
            {
                await _commentRepository.AddAsync(comment);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}