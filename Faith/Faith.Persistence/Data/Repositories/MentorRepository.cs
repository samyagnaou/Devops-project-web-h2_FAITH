using Faith.Core.Interfaces;
using Faith.Core.Models.Roles;
using Microsoft.EntityFrameworkCore;

namespace Faith.Infrastructure.Data.Repositories;

public class MentorRepository : Repository<Mentor>, IMentorRepository
{
    public MentorRepository(FaithDbContext context) : base(context) { }

    public async Task<Mentor?> GetByUserId(string userId)
        => await _dbSet
            .Include(m => m.Students)
            .FirstOrDefaultAsync(s => s.MemberId.Equals(userId));
}