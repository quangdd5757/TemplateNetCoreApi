using Microsoft.AspNetCore.Identity;
using TemplateNetCoreApi.Core.Dtos;
using TemplateNetCoreApi.Core.Models;

namespace TemplateNetCoreApi.Service.Interfaces;

public interface IUserAuthenticationRepository
{
    Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);
    Task<bool> ValidateUserAsync(UserLoginDto loginDto); 
    Task<string> CreateTokenAsync(); 
}

