namespace PasswordManager.Contracts.DTOs;

public class UpdateCredentialRequest
{
    public string Name { get; set; } = default!;

    public string AccountUsername { get; set; } = default!;

    public string AccountPassword { get; set; } = default!;

    public bool IsActive { get; set; }
}
