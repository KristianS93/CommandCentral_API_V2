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

        return Result.Ok(users.Select(u => new UserDTO(u.Id, u.UserName!, u.Email!, u.Firstname!, u.Lastname, u.HouseholdId, "Comming")).ToList());
    }

    public async Task<Result<UserDTO>> GetUser(string id)
    {
        
        var user = await _userManager.FindByIdAsync(id);
        if (user is null || user.Id.IsNullOrEmpty() || user.UserName.IsNullOrEmpty() || user.Email.IsNullOrEmpty())
        {
            return Result.Fail("No user with that id!");
        }

        return Result.Ok(new UserDTO(user!.Id, user.UserName!, user.Email!, user.Firstname, user.Lastname, user.HouseholdId, "Comming"));
    }

    public async Task<Result> DeleteUser(string id)
    {
        var user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user is null)
        {
            return Result.Fail("Could not delete user");
        }
        await _userManager.DeleteAsync(user);
        return Result.Ok();
    }
}