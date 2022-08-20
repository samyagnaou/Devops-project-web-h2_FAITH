﻿using Faith.Core.Models;

namespace Faith.Core.Interfaces
{
    public interface IStudentService
    {
        Task<(bool, Student?)> CreateNewStudent(MemberProfile profile);
        Task<bool> CreateStudentAndAddToGroup(
            string mentorUserId,
            MemberProfile profile);
        Task<IEnumerable<Student>> GetAllStudents();
    }
}