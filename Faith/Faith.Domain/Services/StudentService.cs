﻿using Faith.Core.Interfaces;
using Faith.Core.Models;
using Faith.Core.Models.Roles;

namespace Faith.Core.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IMentorService _mentorService;

    public StudentService(IStudentRepository studentRepository, IMentorService mentorService)
    {
        _studentRepository = studentRepository;
        _mentorService = mentorService;
    }

    public async Task<IEnumerable<Student>> GetAllStudents()
        => await _studentRepository.ListAllAsync();

    public async Task<bool> CreateStudentAndAddToGroup(
        string mentorUserId,
        MemberProfile profile)
    {
        var (isCreated, student) = await CreateNewStudent(profile);
        if (!isCreated)
            return false;
        await _mentorService.AddStudentToGroup(mentorUserId, student!);
        return true;
    }

    public async Task<(bool, Student?)> CreateNewStudent(MemberProfile profile)
    {
        var student = new Student(profile);
        try
        {
            await _studentRepository.AddAsync(student);
            return (true, student);
        }
        catch (Exception) { }
        return (false, null);
    }
}