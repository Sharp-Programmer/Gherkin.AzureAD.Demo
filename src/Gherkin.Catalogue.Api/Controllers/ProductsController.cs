using System.Security.Claims;
using Gherkin.Catalogue.Api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Gherkin.Catalogue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        [Authorize]
        [HttpGet("get")]
        public IActionResult GetAll()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(new[] { "Products.View.All" });
            return Ok(CatalogueDb.Products);
        }

        [Authorize(Roles = "Products.read.all")]
        [HttpGet("get_for_app")]
        public IActionResult GetAllForApp()
        {
            return Ok(CatalogueDb.Products);
        }
    }
}
