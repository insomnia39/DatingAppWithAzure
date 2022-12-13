using DatingApp.DAL;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatingApp.FrontEndAPI.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly ProfileContext _context;

        public BuggyController(ProfileContext context)
        {
            _context ??= context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<User> GetNotFound()
        {
            return new NotFoundResult();
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            try
            {
                var thing = _context.User.Find(-1);

                var thingToReturn = thing.ToString();

                return thingToReturn;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return new BadRequestObjectResult("This was not a good request");
        }
    }
}
