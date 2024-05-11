using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //control hehe
        ManualResetEvent _resetEvent = new ManualResetEvent(false); 
        private readonly IUserService _userService;
    }
}
