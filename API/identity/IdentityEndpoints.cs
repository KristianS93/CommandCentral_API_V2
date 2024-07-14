using API.identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.identity;

public static class IdentityEndpoints
{
    public static WebApplication AddIdentityEndpoints(this WebApplication app)
    {
        var user = app.MapGroup("/user").WithOpenApi();

        user.MapPost("/register", async ([FromBody]RegisterUserDTO user, AuthService authService) =>
        {
            var result = await authService.Register(user);

            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created();
        }).WithName("RegisterUser").AllowAnonymous();

        user.MapPost("/login", async ([FromBody]LoginUserDTO user, AuthService authService) =>
        {
            var result = await authService.Login(user);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(new {token = result.Value});

        }).WithName("LoginUser").AllowAnonymous();

        user.MapGet("/", async (UserService userService) =>
        {
            var result = await userService.Getusers();
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);


        }).RequireAuthorization(Roles.Admin);

        user.MapGet("/{id}", async (string id, UserService userService) =>
        {
            var result = await userService.GetUser(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        }).WithName("GetUser").RequireAuthorization(Roles.Admin);

        user.MapPut("/{id}", async (string id) =>
        {
            await Task.Delay(1);


        }).WithName("EditUser").RequireAuthorization(Roles.Member);

        user.MapDelete("/{id}", async (string id, UserService userService) =>
        {
            var result = await userService.DeleteUser(id);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok();

        }).WithName("DeleteUser").RequireAuthorization(Roles.Admin);
        
        return app;
    }
}