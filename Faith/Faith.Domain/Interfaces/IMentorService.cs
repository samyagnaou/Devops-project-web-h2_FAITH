using Faith.Core.Models;

namespace Faith.Core.Interfaces
{
    public interface IMentorService
    {
        Task<IEnumerable<Student>> GetStudentsInGroup(string userId);
        Task<bool> CreateNewMentor(MemberProfile memberDetails);
        Task<bool> AddStudentToGroup(string mentorUserId, Student student);
        Task<bool> AddStudentToGroup(string mentorUserId, string studentUserId);
        Task<bool> RemoveStudentFromGroup(string mentorUserId, string studentUserId);
    }
}