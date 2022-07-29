using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.SDK;

namespace PasswordManager.UI.Controllers;

[Authorize]
public class CredentialsController : Controller
{
    private readonly IPasswordManagerApi _api;

    public CredentialsController(IPasswordManagerApi api)
    {
        _api = api;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _api.GetAllCredentials("1");

        return View();
    }
}
