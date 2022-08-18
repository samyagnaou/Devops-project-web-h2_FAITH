using Faith.Core.Interfaces;
using Faith.Core.Models.Roles;
using Faith.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    [Authorize(Roles = Roles.Mentor)]
    public class StudentsController : ApiControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;

        public StudentsController(
            IMentorService mentorService,
            IStudentService studentService)
        {
            _studentService = studentService;
            _mentorService = mentorService;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _studentService.GetAllStudents();
        }

        [HttpGet("group")]
        public async Task<IEnumerable<Student>> GetStudentsInMentorGroup()
        {
            return await _mentorService.GetStudentsInGroup(User!.Identity!.Name!);
        }

        [HttpPost("add-to-group")]
        public async Task<IActionResult> AddStudentToGroup([FromBody] string studentUserId)
        {
            var isAdded = await _mentorService
                .AddStudentToGroup(User!.Identity!.Name!, studentUserId);
            if (!isAdded)
                return UnprocessableEntity();
            return Ok();
        }

        [HttpPost("remove-from-group")]
        public async Task<IActionResult> RemoveStudentFromGroup([FromBody] string studentUserId)
        {
            var isRemoved = await _mentorService
                .RemoveStudentFromGroup(User!.Identity!.Name!, studentUserId);
            if (!isRemoved)
                return UnprocessableEntity();
            return Ok();
        }

    }
}
