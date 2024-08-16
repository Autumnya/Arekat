using Microsoft.AspNetCore.Mvc;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("test")]
        public string Get()
        {
            return "test";
        }
    }
}