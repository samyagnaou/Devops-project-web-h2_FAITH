using Faith.Core.Models;

namespace Faith.Core.Interfaces;

public interface IStudentService
{
    Task<bool> CreateNewStudent(MemberDetails memberDetails);
}