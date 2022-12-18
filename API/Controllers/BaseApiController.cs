using DatingApp.BLL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.FrontEndAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
         
    }
}
