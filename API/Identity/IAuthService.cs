using API.identity.Models;
using FluentResults;

namespace API.identity;

public interface IAuthService
{
    Task<Result> Register(RegisterUserDto request);
    Task<Result<string>> Login(LoginUserDto request);
}