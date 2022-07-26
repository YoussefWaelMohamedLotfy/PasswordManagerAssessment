﻿using AutoMapper;
using PasswordManager.API.Data.Repositories;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API.Endpoints.V1.Credentials;

public class GetAllCredentials : EndpointBaseAsync.WithoutRequest.WithActionResult<GetAllCredentialsResponse>
{
    private readonly ISocialCredentialRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCredentials(ISocialCredentialRepository repo, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("api/[namespace]")]
    public override async Task<ActionResult<GetAllCredentialsResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var credentialsinDb = await _repo.GetAllAsync(cancellationToken);

        var result = _mapper.Map<IReadOnlyList<GetAllCredentialsResponse>>(credentialsinDb);
        return Ok(result);
    }
}
