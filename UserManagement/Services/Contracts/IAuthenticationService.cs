﻿using System.Threading.Tasks;
using UserManagementService.Dtos;
using UserManagementService.Dtos.ChiliUser;

namespace UserManagementService.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<ChiliUserDto> RegisterAsync(UserRegistrationDto request);
        Task<AuthenticationDto> LoginAsync(UserLoginDto request);
        Task<AuthenticationDto> VerifyTokenAsync(string token, string refreshToken);
    }
}
