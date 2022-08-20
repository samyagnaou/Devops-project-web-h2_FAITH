using Faith.Core.Interfaces;
using Faith.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(FaithPlatformContext context) : base(context) { }

        public async Task<Message?> GetMessageById(int id)
        {
            return await _dbSet
                .Include(m => m.Student)
                .ThenInclude(s => s.Mentors)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Message>> GetAllMessagesForAStudent(string studentUserId)
        {
            var student = await _context.Students
                .Include(s => s.Messages)
                .ThenInclude(m => m.Comments)
                .ThenInclude(c => c.Mentor)
                .FirstOrDefaultAsync(s => s.MemberId == studentUserId);

            return student!.Messages;
        }


        public async Task<IEnumerable<Message>> GetAllMessagesInMentorGroup(string mentorUserId)
        {
            var mentor = await _context
                .Mentors
                .Include(m => m.Students)
                .ThenInclude(s => s.Messages)
                .ThenInclude(m => m.Comments)
                .FirstOrDefaultAsync(m => m.MemberId == mentorUserId);

            return mentor!.Students.SelectMany(s => s.Messages);
        }
    }
}