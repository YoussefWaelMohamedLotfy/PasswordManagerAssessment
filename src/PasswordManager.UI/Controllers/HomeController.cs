using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.UI.Models;
using System.Diagnostics;

namespace PasswordManager.UI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() 
        => View();

    [Authorize]
    public IActionResult Claims() 
        => View();

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