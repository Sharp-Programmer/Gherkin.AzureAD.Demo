using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gherkin.Catalogue.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            return Ok(new {message = "Gherkin Catalogue Api is running"});
        }
    }
}
