using AutoMapper;
using FluentValidation;
using PasswordManager.API.Data.Repositories;
using PasswordManager.API.Models;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class CreateCredential : EndpointBaseAsync.WithRequest<CreateCredentialRequest>.WithActionResult<CreateCredentialResponse>
{
    private readonly ISocialCredentialRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCredentialRequest> _validator;

    public CreateCredential(ISocialCredentialRepository repo, IMapper mapper, IValidator<CreateCredentialRequest> validator)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
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
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public override async Task<ActionResult<CreateCredentialResponse>> HandleAsync([FromBody] CreateCredentialRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var newCredential = _mapper.Map<SocialCredential>(request);
        await _repo.AddAsync(newCredential);

        var result = _mapper.Map<CreateCredentialResponse>(newCredential);
        return CreatedAtRoute("Credentials_GetSingleCredential", new { id = result.ID }, result);
    }
}
