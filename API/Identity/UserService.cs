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

        return Result.Ok(users.Select(u => new UserDTO(u.Id, u.UserName!, u.Email!, u.Firstname!, u.Lastname, "Comming")).ToList());
    }

    public async Task<Result<UserDTO>> GetUser(string id)
    {
        
        var user = await _userManager.FindByIdAsync(id);
        if (user is null || user.Id.IsNullOrEmpty() || user.UserName.IsNullOrEmpty() || user.Email.IsNullOrEmpty())
        {
            return Result.Fail("No user with that id!");
        }

        return Result.Ok(new UserDTO(user!.Id, user.UserName!, user.Email!, user.Firstname, user.Lastname, "Comming"));
    }

    public async Task<Result> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return Result.Fail("Could not delete user");
        }
        await _userManager.DeleteAsync(user);
        return Result.Ok();
    }

    public async Task<Result> EditUser(UserDTO user)
    {
        var identity = await _userManager.FindByIdAsync(user.userId);
        if (identity is null)
        {
            return Result.Fail("User does not exist");
        }
        identity.Firstname = user.firstname;
        identity.Lastname = user.lastname;
        identity.UserName = user.username;
        identity.Email = user.email;
        var x = await _userManager.UpdateAsync(identity);
        if (!x.Succeeded)
        {
            return Result.Fail(x.Errors.Select(e => new Error(e.Description)));
        }

        return Result.Ok();
    }

    public async Task<Result> UpdateRoleAdmin(RoleDTO role)
    {
        var identity = await _userManager.FindByIdAsync(role.userid);
        if (identity is null)
        {
            return Result.Fail("No user with that id");
        }
        var currentRole = await _userManager.GetRolesAsync(identity);
        if (currentRole.IsNullOrEmpty())
        {
            return Result.Fail("No previous roles exist");
        }
        await _userManager.RemoveFromRoleAsync(identity, currentRole.First());
        var newRole = role.role switch
        {
            "Member" => Roles.Member,
            "Owner" => Roles.Owner,
            "Administrator" => Roles.Admin,
            _ => Roles.Member
        };

        await _userManager.AddToRoleAsync(identity, newRole);

        return Result.Ok();
    }
}