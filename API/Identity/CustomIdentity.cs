using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using API.identity;
using API.identity.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Identity;

public static class CustomIdentity
{
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    public static IEndpointConventionBuilder MapCustomIdentity(this IEndpointRouteBuilder endpoints)
    {
        var routeGroup = endpoints.MapGroup("");
        var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
        var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
        
        // Register
        routeGroup.MapPost("/register", async Task<Results<Created, ValidationProblem>>(UserManager<CCAIdentity> userManager, [FromBody]RegisterUserDto registration) =>
        {

            if (registration.Password != registration.VerifyPassword)
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.PasswordMismatch()));
            }
            
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(MapCustomIdentity)} requires a user store with email support.");
            }
            
            var email = registration.Email;
            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
            }

            var user = new CCAIdentity()
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                EmailConfirmed = true,
            };
            await userManager.SetUserNameAsync(user, email);
            await userManager.SetEmailAsync(user, email);
            var result = await userManager.CreateAsync(user, registration.Password);
            await userManager.AddToRoleAsync(user, Roles.Member);
            
            if (!result.Succeeded)
            {
                return CreateValidationProblem(result);
            }
            return TypedResults.Created();
        });
        
        routeGroup.MapPost("/login",
            async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> (
                [FromBody] LoginUserDto loginRequest, UserManager<CCAIdentity> userManager, SignInManager<CCAIdentity> signInManager) =>
            {
                signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
                var result = await signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
                
                if (!result.Succeeded)
                {
                    return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
                }
                
                // The signInManager already produced the needed response in the form of a cookie or bearer token.
                return TypedResults.Empty;
            });
        routeGroup.MapPost("/refresh", async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>> ([FromBody]RefreshDto refresh, SignInManager<CCAIdentity> signInManager) =>
        {
            var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
            var refreshTicket = refreshTokenProtector.Unprotect(refresh.RefreshToken);

            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                timeProvider.GetUtcNow() >= expiresUtc ||
                await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not CCAIdentity user)
            {
                return TypedResults.Challenge();
            }
            
            var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
            return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
        });
        
        return new IdentityEndpointsConventionBuilder(routeGroup);
    }
    
    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
    
    private sealed class IdentityEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
    {
        private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

        public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
        public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
    }
    
    private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
        TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
        });


}