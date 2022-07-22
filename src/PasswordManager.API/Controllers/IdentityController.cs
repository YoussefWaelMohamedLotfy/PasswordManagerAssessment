using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IdentityController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() 
        => new JsonResult(from c in User.Claims select new { c.Type, c.Value });
}
