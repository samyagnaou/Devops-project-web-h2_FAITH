using Faith.Core.Interfaces;
using Faith.Core.Models;

namespace Faith.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentService(
            IMessageRepository messageRepository,
            ICommentRepository commentRepository)
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
                CreatedAt = DateTime.Now
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

        public async Task<bool> EditComment(string userId, int id, string text)
        {
            var comment = await _commentRepository.GetCommentById(id);
            if (comment == null)
                return false;
            if (comment.Mentor?.MemberId != userId && comment.Student?.MemberId != userId)
                return false;
            try
            {
                comment.Text = text;
                comment.CreatedAt = DateTime.Now;
                await _commentRepository.UpdateAsync(comment);
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> DeleteComment(string userId, int id)
        {
            var comment = await _commentRepository.GetCommentById(id);
            if (comment == null)
                return false;
            if (comment.Mentor?.MemberId != userId && comment.Student?.MemberId != userId)
                return false;
            try
            {
                await _commentRepository.RemoveAsync(comment);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}