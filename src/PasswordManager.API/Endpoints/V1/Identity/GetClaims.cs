namespace PasswordManager.API.Endpoints.V1.Identity;

public class GetClaims : EndpointBaseSync.WithoutRequest.WithActionResult
{
    /// <summary>
    /// Gets Claims
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/[namespace]")]
    [Authorize]
    public override ActionResult Handle()
       => new JsonResult(from c in User.Claims select new { c.Type, c.Value });
}
