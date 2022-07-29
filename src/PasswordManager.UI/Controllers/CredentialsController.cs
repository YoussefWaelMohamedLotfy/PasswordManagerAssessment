using Microsoft.AspNetCore.Mvc;
using PasswordManager.SDK;
using PasswordManager.UI.Models;
using Refit;

namespace PasswordManager.UI.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
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
        var subjectIdFromClaim = User.FindFirst("sub")!.Value;
        var response = await _api.GetAllCredentials(subjectIdFromClaim);

        CredentialIndexVM viewModel = new();

        if (response.IsSuccessStatusCode)
        {
            viewModel.Credentials = response.Content;
        }

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Upsert(int? id)
    {
        CredentialUpsertVM viewModel = new();

        if (id is null)
        {
            // Create
            viewModel.Credential = new();
        }
        else
        {
            // Update
            var response = await _api.GetSingleCredential(id.Value);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            viewModel.Credential = response.Content!;
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(CredentialUpsertVM viewModel)
    {
        IApiResponse response;

        if (viewModel.Credential.ID == 0)
        {
            var subjectIdFromClaim = User.FindFirst("sub")!.Value;

            response = await _api.CreateCredential(new()
            {
                SubjectID = subjectIdFromClaim,
                Name = viewModel.Credential.Name,
                AccountUsername = viewModel.Credential.AccountUsername,
                AccountPassword = viewModel.Credential.AccountPassword,
                IsActive = viewModel.Credential.IsActive,
            });
        }
        else
        {
            response = await _api.UpdateCredential(viewModel.Credential.ID, new()
            {
                Name = viewModel.Credential.Name,
                AccountUsername = viewModel.Credential.AccountUsername,
                AccountPassword = viewModel.Credential.AccountPassword,
                IsActive = viewModel.Credential.IsActive,
            });
        }

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));
        else
            return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCredential(int id)
    {
        var response = await _api.DeleteCredential(id);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));
        else 
            return View(nameof(Index));
    }
}
