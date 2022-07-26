﻿using AutoMapper;
using PasswordManager.API.Models;
using PasswordManager.Contracts.DTOs;

namespace PasswordManager.API;

public class SocialCredentialMappingProfile : Profile
{
    public SocialCredentialMappingProfile()
    {
        CreateMap<SocialCredential, GetAllCredentialsResponse>();
        CreateMap<SocialCredential, GetSingleCredentialResponse>();

        CreateMap<CreateCredentialRequest, SocialCredential>();
        CreateMap<SocialCredential, CreateCredentialResponse>();

        CreateMap<UpdateCredentialRequest, SocialCredential>()
            .ForMember(d => d.Name, o => o.Ignore());
        CreateMap<SocialCredential, UpdateCredentialResponse>();
    }
}
