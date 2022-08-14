using Faith.Core.Interfaces;
using Faith.Core.Models.Roles;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(FaithDbContext context) : base(context) { }

    public async Task<Student?> GetByUserId(string userId)
        => await _dbSet.Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.MemberId.Equals(userId));
}