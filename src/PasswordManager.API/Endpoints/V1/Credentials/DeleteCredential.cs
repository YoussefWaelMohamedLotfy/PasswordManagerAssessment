using PasswordManager.API.Data.Repositories;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class DeleteCredential : EndpointBaseAsync.WithRequest<int>.WithActionResult
{
    private readonly ISocialCredentialRepository _repo;

    public DeleteCredential(ISocialCredentialRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    /// <summary>
    /// Deletes a Credential with ID from Database
    /// </summary>
    /// <param name="id">The credential's ID</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Credential Deleted</response>
    /// <response code="404">No Credential found</response>
    /// <returns></returns>
    [HttpDelete("api/[namespace]/{id:int}")]
    public override async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var credentialInDb = await _repo.GetByIdAsync(id, cancellationToken);

        if (credentialInDb is null)
            return NotFound();

        await _repo.DeleteAsync(credentialInDb, cancellationToken);

        return NoContent();
    }
}
