using API.identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.identity;

public static class IdentityEndpoints
{
    public static WebApplication AddIdentityEndpoints(this WebApplication app)
    {
        var user = app.MapGroup("/user").WithOpenApi();

        user.MapPost("/register", async ([FromBody]RegisterUserDTO user) =>
        {
            
        }).WithName("RegisterUser").AllowAnonymous();

        user.MapPost("/login", async ([FromBody]LoginUserDTO user) =>
        {
            
        }).WithName("LoginUser").AllowAnonymous();

        user.MapGet("/", async () =>
        {

        }).WithName("GetUsers").RequireAuthorization(Roles.Admin);

        user.MapGet("/{id}", async (string id) =>
        {

        }).WithName("GetUser").RequireAuthorization(Roles.Admin);

        user.MapPut("/{id}", async (string id) =>
        {

        }).WithName("EdituUser").RequireAuthorization(Roles.Member);

        user.MapDelete("/{id}", async (string id) =>
        {

        }).WithName("DeleteUser").RequireAuthorization(Roles.Admin);
        
        return app;
    }
}