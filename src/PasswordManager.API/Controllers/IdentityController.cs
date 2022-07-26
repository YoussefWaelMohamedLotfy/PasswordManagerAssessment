using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IdentityController : ControllerBase
{

    /// <summary>
    /// Gets Logged in user's Claims
    /// </summary>
    /// <returns>User's Claims</returns>
    [HttpGet]
    public IActionResult Get() 
        => new JsonResult(from c in User.Claims select new { c.Type, c.Value });
}
