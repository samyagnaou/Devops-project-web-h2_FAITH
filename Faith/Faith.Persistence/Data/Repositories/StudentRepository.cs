using Faith.Core.Interfaces;
using Faith.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(FaithPlatformContext context) : base(context) { }

        public async Task<Student?> GetByUserId(string userId)
            => await _dbSet.FirstOrDefaultAsync(s => s.MemberId.Equals(userId));
    }
}