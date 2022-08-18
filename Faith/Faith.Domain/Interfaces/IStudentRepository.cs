using Faith.Core.Models.Roles;

namespace Faith.Core.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByUserId(string userId);
}