using Faith.Core.Models.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    public class StudentsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Student>> GetAll()
        {
            return Enumerable.Empty<Student>();
        }
    }
}
