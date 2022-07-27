using AutoMapper;
using PasswordManager.API.Data.Repositories;
using PasswordManager.API.Services;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class GetSingleCredential : EndpointBaseAsync.WithRequest<int>.WithActionResult<GetSingleCredentialResponse>
{
    private readonly ISocialCredentialRepository _repo;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptor;

    public GetSingleCredential(ISocialCredentialRepository repo, IMapper mapper, IEncryptionService encryptor)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
    }

    /// <summary>
    /// Gets a single Credential with ID
    /// </summary>
    /// <param name="id">The credential's ID</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Credentials Retrieved</response>
    /// <response code="404">No Credential found</response>
    /// <returns>A credential of specified ID</returns>
    [HttpGet("api/[namespace]/{id:int}", Name = "[namespace]_[controller]")]
    public override async Task<ActionResult<GetSingleCredentialResponse>> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var credentialInDb = await _repo.GetByIdAsync(id, cancellationToken);

        if (credentialInDb is null)
            return NotFound();

        credentialInDb.AccountPassword = _encryptor.DecryptString(credentialInDb.AccountPassword);

        var result = _mapper.Map<GetSingleCredentialResponse>(credentialInDb);
        return Ok(result);
    }
}
