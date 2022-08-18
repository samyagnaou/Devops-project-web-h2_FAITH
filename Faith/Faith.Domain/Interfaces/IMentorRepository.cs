using Faith.Core.Models.Roles;

namespace Faith.Core.Interfaces;

public interface IMentorRepository : IRepository<Mentor>
{
    Task<Mentor?> GetByUserId(string userId);
}