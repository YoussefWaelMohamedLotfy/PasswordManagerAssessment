namespace PasswordManager.Contracts.DTOs;

public class CreateCredentialRequest
{
    public string SubjectID { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string AccountUsername { get; set; } = default!;

    public string AccountPassword { get; set; } = default!;

    public bool IsActive { get; set; }
}
