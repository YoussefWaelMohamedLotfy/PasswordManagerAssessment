using AutoMapper;
using PasswordManager.API.Data.Repositories;
using PasswordManager.API.Models;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class UpdateCredential : EndpointBaseAsync.WithRequest<UpdateRequest>.WithActionResult<UpdateCredentialResponse>
{
    private readonly ISocialCredentialRepository _repo;
    private readonly IMapper _mapper;

    public UpdateCredential(ISocialCredentialRepository repo, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Updates an existing Credential record in Database
    /// </summary>
    /// <param name="request">Incoming updated Credential request</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Updated Credential Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="404">Credential Not Found</response>
    /// <returns>The updated credential</returns>
    [HttpPut("api/[namespace]/{id:int}")]
    [ProducesResponseType(typeof(UpdateCredentialResponse), 200)]
    public override async Task<ActionResult<UpdateCredentialResponse>> HandleAsync([FromRoute] UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var credentialInDb = await _repo.GetByIdAsync(request.Id);

        if (credentialInDb is null)
            return NotFound();

        _mapper.Map(request.RequestBody, credentialInDb);
        
        if (Enum.TryParse(request.RequestBody.Name, out CredentialAppName parsedValue))
            credentialInDb.Name = parsedValue;

        await _repo.UpdateAsync(credentialInDb);

        var result = _mapper.Map<UpdateCredentialResponse>(credentialInDb);
        return Ok(result);
    }
}

public class UpdateRequest
{
    [FromRoute] public int Id { get; set; }

    [FromBody] public UpdateCredentialRequest RequestBody { get; set; } = default!;
}