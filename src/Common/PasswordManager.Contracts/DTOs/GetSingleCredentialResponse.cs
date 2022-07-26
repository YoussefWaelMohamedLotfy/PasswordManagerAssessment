namespace PasswordManager.Contracts.DTOs;

public class GetSingleCredentialResponse
{
    public int ID { get; set; }

    public string SubjectID { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string AccountUsername { get; set; } = default!;

    public string AccountPassword { get; set; } = default!;

    public bool IsActive { get; set; }
}
