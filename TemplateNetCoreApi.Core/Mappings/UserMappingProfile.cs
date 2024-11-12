using AutoMapper;
using TemplateNetCoreApi.Core.Dtos;
using TemplateNetCoreApi.Core.Models;

namespace TemplateNetCoreApi.Core.Mappings;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserRegistrationDto, User>();
    }
}
