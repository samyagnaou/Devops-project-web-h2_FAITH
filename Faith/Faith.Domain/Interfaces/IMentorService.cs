using Faith.Core.Models;
using Faith.Core.Models.Roles;

namespace Faith.Core.Interfaces;

public interface IMentorService
{
    Task<bool> CreateNewMentor(MemberProfile memberProfile);
    Task<bool> AddStudentToGroup(string mentorUserId, Student student);
    Task<bool> AddStudentToGroup(string mentorUserId, string studentUserId);
    Task<bool> RemoveStudentFromGroup(string mentorUserId, string studentUserId);
    Task<IEnumerable<Student>> GetStudentsInGroup(string userId);
}