using PasswordManager.Contracts.DTOs;

namespace PasswordManager.UI.Models;

public class CredentialUpsertVM
{
    public GetSingleCredentialResponse Credential { get; set; } = default!;
}
