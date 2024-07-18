using API.identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Identity;

public class UserDomainValidator<TUser> : IUserValidator<CCAIdentity> where TUser: IdentityUser
{
    public Task<IdentityResult> ValidateAsync(UserManager<CCAIdentity> manager, CCAIdentity user)
    {
        if (user.FirstName.IsNullOrEmpty() || user.LastName.IsNullOrEmpty())
        {
            return Task.FromResult(
                IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidName",
                    Description = "Invalid First or Last name."
                }));
        }

        if (user.FirstName.Length < 2 || user.LastName.Length < 2)
        {
            return Task.FromResult(
                IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidNameLength",
                    Description = "Names have to be at least 2 characters"
                }));
        }

        var allowedChars = "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZØÆÅ-";
        if (!IsStringWithinAllowedChars(user.FirstName, allowedChars) ||
            !IsStringWithinAllowedChars(user.LastName, allowedChars))
        {
            return Task.FromResult(
                IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidChar",
                    Description = "Names can only contains a-å, A-Å and '-'."
                }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
    
    public static bool IsStringWithinAllowedChars(string input, string allowedChars)
    {
        HashSet<char> allowedSet = new HashSet<char>(allowedChars);

        foreach (char c in input)
        {
            if (!allowedSet.Contains(c))
            {
                return false;
            }
        }

        return true;
    }
}