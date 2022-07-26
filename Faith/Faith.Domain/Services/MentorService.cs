﻿using Faith.Core.Interfaces;
using Faith.Core.Models;

namespace Faith.Core.Services
{
    public class MentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly IStudentRepository _studentRepository;

        public MentorService(
            IMentorRepository mentorRepository,
            IStudentRepository studentRepository)
        {
            _mentorRepository = mentorRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetStudentsInGroup(string userId)
        {
            var mentor = await _mentorRepository.GetMentorAndStudentsByUserId(userId);
            if (mentor != null)
            {
                return mentor.Students;
            }
            return Enumerable.Empty<Student>();
        }

        public async Task<bool> AddStudentToGroup(string mentorUserId, string studentUserId)
        {
            var mentor = await _mentorRepository.GetMentorAndStudentsByUserId(mentorUserId);
            if (mentor == null)
                return false;
            var student = await _studentRepository.GetByUserId(studentUserId);
            if (student == null)
                return false;
            mentor.Students.Add(student);
            try
            {
                await _mentorRepository.UpdateAsync(mentor);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> RemoveStudentFromGroup(string mentorUserId, string studentUserId)
        {
            var mentor = await _mentorRepository.GetMentorAndStudentsByUserId(mentorUserId);
            if (mentor == null)
                return false;
            var student = mentor.Students.FirstOrDefault(s => s.MemberId == studentUserId);
            if (student == null)
                return false;
            mentor.Students.Remove(student);
            try
            {
                await _mentorRepository.UpdateAsync(mentor);
                return true;
            }
            catch { return false; }
        }


        public async Task<bool> AddStudentToGroup(string mentorUserId, Student student)
        {
            var mentor = await _mentorRepository.GetMentorAndStudentsByUserId(mentorUserId);
            if (mentor == null)
                return false;
            mentor.Students.Add(student);
            try
            {
                await _mentorRepository.UpdateAsync(mentor);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> CreateNewMentor(MemberProfile profile)
        {
            var mentor = new Mentor(profile);
            try
            {
                await _mentorRepository.AddAsync(mentor);
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public async Task<bool> UpdateMemberProfile(string userId, MemberProfile profile)
        {
            var mentor = await _mentorRepository.GetMentorByUserId(userId);
            if (mentor == null)
                return false;
            mentor.FirstName = profile.FirstName;
            mentor.LastName = profile.LastName;
            mentor.BirthDate = profile.BirthDate;
            try
            {
                await _mentorRepository.UpdateAsync(mentor);
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<Mentor?> GetMentorByUserId(string userId)
            => await _mentorRepository.GetMentorByUserId(userId);
    }
}