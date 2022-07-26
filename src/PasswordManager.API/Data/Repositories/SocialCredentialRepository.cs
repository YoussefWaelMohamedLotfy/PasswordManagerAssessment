using Microsoft.EntityFrameworkCore;
using PasswordManager.API.Models;

namespace PasswordManager.API.Data.Repositories;

public class SocialCredentialRepository : ISocialCredentialRepository
{
    private readonly AppDbContext _context;

    public SocialCredentialRepository(AppDbContext context)
        => _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IReadOnlyList<SocialCredential>> GetAllAsync(string subjectID, CancellationToken cancellationToken = default)
        => await _context.SocialCredentials.AsNoTracking().Where(x => x.SubjectID == subjectID).ToListAsync(cancellationToken);

    public async Task<SocialCredential> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => (await _context.SocialCredentials.FindAsync(id, cancellationToken))!;

    public async Task<SocialCredential> AddAsync(SocialCredential entity, CancellationToken cancellationToken = default)
    {
        await _context.SocialCredentials.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SocialCredential entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SocialCredential entity)
    {
        _context.SocialCredentials.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
