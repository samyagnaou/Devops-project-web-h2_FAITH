using Faith.Core.Models.Roles;

namespace Faith.Core.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetByUserId(string userId);
}