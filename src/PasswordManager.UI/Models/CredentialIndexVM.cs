using PasswordManager.Contracts.DTOs;

namespace PasswordManager.UI.Models;

public class CredentialIndexVM
{
    public List<GetAllCredentialsResponse> Credentials { get; set; } = default!;
}
