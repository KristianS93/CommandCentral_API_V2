

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.identity.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.identity;

public class AuthService : IAuthService
{
    private readonly IConfiguration _confManager;
    private readonly UserManager<CCAIdentity> _userManager;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly SignInManager<CCAIdentity> _signInManager;
    public AuthService(IConfiguration confManager, UserManager<CCAIdentity> userManager, IOptions<JwtSettings> jwtSettings, SignInManager<CCAIdentity> signInManager)
    {
        _confManager = confManager;
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _signInManager = signInManager;
    }

    public async Task<Result<string>> Login(LoginUserDTO request)
    {
        var error = "Invalid Credentials!";
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == request.username);
        if (user is null)
        {
            return Result.Fail(error);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.password, false);
        if (!result.Succeeded)
        {
            return Result.Fail(error);
        }

        JwtSecurityToken token = await GenerateToken(user);
        var strToken = new JwtSecurityTokenHandler().WriteToken(token);
        Console.WriteLine(strToken);
        return Result.Ok(strToken);
    }

    public async Task<Result> Register(RegisterUserDTO request)
    {
        if (request.password != request.verifyPassword)
        {
            var errors = new List<Error>();
            errors.Add(new Error("Passwords does not match!"));
            return Result.Fail(errors);
        }
        var user = new CCAIdentity
        {
            Email = request.email,
            Firstname = request.firstname,
            Lastname = request.lastname,
            UserName = request.username,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.password);
        if (!result.Succeeded)
        {
            var x = result.Errors.Select(e => new Error(e.Description));
            return Result.Fail(x);
        }
        // add role to new user
        var getUser = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == user.UserName);
        await _userManager.AddToRoleAsync(getUser!, Roles.Member);
        
        return Result.Ok();
    }
    
    private async Task<JwtSecurityToken> GenerateToken(CCAIdentity user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);
        var roleClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_confManager["JwtSettings:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim("uid", user.Id),
            
        }.Union(userClaims).Union(roleClaims);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.Value.DurationInMinutes),
            signingCredentials: credentials
            );
        return token;
    }
}