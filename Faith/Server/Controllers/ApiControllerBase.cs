using Microsoft.AspNetCore.Mvc;

namespace Faith.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiControllerBase : ControllerBase { }
}
