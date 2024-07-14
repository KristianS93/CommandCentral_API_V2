using API.identity.Models;
using FluentResults;

namespace API.identity;

public interface IAuthService
{
    Task<Result> Register(RegisterUserDTO request);
    Task<Result<string>> Login(LoginUserDTO request);
}