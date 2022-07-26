using PasswordManager.API.Models;

namespace PasswordManager.API.Data.Repositories;

public interface ISocialCredentialRepository
{
    Task<SocialCredential> AddAsync(SocialCredential entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(SocialCredential entity);
    Task<IReadOnlyList<SocialCredential>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SocialCredential> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(SocialCredential entity);
}