namespace PasswordManager.API.Models;

public class SocialCredential
{
    public int ID { get; set; }

    public string SubjectID { get; set; } = default!;
    
    public CredentialAppName Name { get; set; } = default!;
    
    public string AccountUsername { get; set; } = default!;
    
    public string AccountPassword { get; set; } = default!;

    public bool IsActive { get; set; }
}
