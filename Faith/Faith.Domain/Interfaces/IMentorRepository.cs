using Faith.Core.Models;

namespace Faith.Core.Interfaces
{
    public interface IMentorRepository : IRepository<Mentor>
    {
        Task<Mentor?> GetMentorAndStudentsByUserId(string userId);
    }
}