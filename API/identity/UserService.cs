using API.identity.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.identity;

public class UserService
{
    private readonly UserManager<CCAIdentity> _userManager;
    public UserService(UserManager<CCAIdentity> userManager)
    {
        _userManager = userManager;
    }
    public async Task<Result<List<UserDTO>>> Getusers()
    {
        var users = await _userManager.Users.ToListAsync();
        if (users.IsNullOrEmpty())
        {
            return Result.Ok(new List<UserDTO>());
        }

        return Result.Ok(users.Select(u => new UserDTO(u.Id, u.UserName!, u.Email!, u.Firstname!, u.Lastname, u.HouseholdId, "Commin")).ToList());
    }
}