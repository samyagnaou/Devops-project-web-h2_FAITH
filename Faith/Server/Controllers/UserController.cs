using Faith.Shared.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public Task<UserResponse.GetIndex> GetIndexAsync([FromQuery] UserRequest.GetIndex request)
        {
            return userService.GetIndexAsync(request);
        }
    }
}
