using AutoMapper;
using PasswordManager.API.Data.Repositories;
using PasswordManager.API.Models;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class CreateCredential : EndpointBaseAsync.WithRequest<CreateCredentialRequest>.WithActionResult<CreateCredentialResponse>
{
    private readonly ISocialCredentialRepository _repo;
    private readonly IMapper _mapper;

    public CreateCredential(ISocialCredentialRepository repo, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Creates a new Credential record in Database
    /// </summary>
    /// <param name="request">Incoming Credential request</param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">Created Credential Success</response>
    /// <response code="400">Bad Request</response>
    /// <returns>The created credential in Database</returns>
    [HttpPost("api/[namespace]")]
    [ProducesResponseType(typeof(CreateCredentialResponse), 201)]
    public override async Task<ActionResult<CreateCredentialResponse>> HandleAsync([FromBody] CreateCredentialRequest request, CancellationToken cancellationToken = default)
    {
        var newCredential = _mapper.Map<SocialCredential>(request);
        await _repo.AddAsync(newCredential);

        var result = _mapper.Map<CreateCredentialResponse>(newCredential);
        return CreatedAtRoute("Credentials_GetSingleCredential", new { id = result.ID }, result);
    }
}
