using AutoMapper;
using PasswordManager.API.Data.Repositories;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class GetAllCredentials : EndpointBaseAsync.WithRequest<string>.WithActionResult<GetAllCredentialsResponse>
{
    private readonly ISocialCredentialRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCredentials(ISocialCredentialRepository repo, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Get All Credentials for a specific Subject ID
    /// </summary>
    /// <param name="subjectId">The User's ID in Identity Server</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Credentials Retrieved</response>
    /// <returns>A list of credentials</returns>
    [HttpGet("api/[namespace]")]
    public override async Task<ActionResult<GetAllCredentialsResponse>> HandleAsync(string subjectId, CancellationToken cancellationToken = default)
    {
        var credentialsinDb = await _repo.GetAllAsync(subjectId, cancellationToken);

        var result = _mapper.Map<IReadOnlyList<GetAllCredentialsResponse>>(credentialsinDb);
        return Ok(result);
    }
}
