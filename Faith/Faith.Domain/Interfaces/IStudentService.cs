using Faith.Core.Models;
using Faith.Core.Models.Roles;

namespace Faith.Core.Interfaces;

public interface IStudentService
{
    Task<(bool, Student?)> CreateNewStudent(MemberProfile profile);
    Task<bool> CreateStudentAndAddToGroup(
        string mentorUserId,
        MemberProfile profile);
    Task<IEnumerable<Student>> GetAllStudents();
}