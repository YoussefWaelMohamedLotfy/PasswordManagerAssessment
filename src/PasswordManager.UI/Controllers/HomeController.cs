using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.SDK;
using PasswordManager.UI.Models;
using System.Diagnostics;

namespace PasswordManager.UI.Controllers;

public class HomeController : Controller
{
    private readonly IPasswordManagerApi _api;

    public HomeController(IPasswordManagerApi api)
    {
        _api = api;
    }

    public IActionResult Index() 
        => View();

    [Authorize]
    public IActionResult Claims() 
        => View();

    [Authorize]
    public IActionResult BackendTest()
        => View();

    public async Task<IActionResult> GetSingle()
    {
        var response = await _api.GetSingleCredential(20);

        return View(nameof(BackendTest));
    }

    public async Task<IActionResult> GetAll()
    {
        var response = await _api.GetAllCredentials("1");

        return View(nameof(BackendTest));
    }

    public async Task<IActionResult> Create()
    {
        var response = await _api.CreateCredential(new()
        {
            SubjectID = "1",
            Name = "Microsoft",
            AccountUsername = "Hello",
            AccountPassword = "1234456",
            IsActive = true
        });

        return View(nameof(BackendTest));
    }

    public async Task<IActionResult> Update()
    {
        var response = await _api.UpdateCredential(19, new()
        {
            Name = "Microsoft",
            AccountUsername = "Nogo.207",
            AccountPassword = "12184t256",
            IsActive = false
        });

        return View(nameof(BackendTest));
    }

    public async Task<IActionResult> Delete()
    {
        var response = await _api.DeleteCredential(19);

        return View(nameof(BackendTest));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}