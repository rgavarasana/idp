using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ravi.learn.identity.api.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetValues()
        {
            return  Content($"Welcome {User.Identity.Name}");
        }

        [HttpGet]
        [Route("User")]
        [Authorize]
        public IActionResult GetUser()
        {
            return Content($"User:  {User.Identity.Name}");
        }
    }
}