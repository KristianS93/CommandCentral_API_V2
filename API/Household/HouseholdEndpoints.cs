using System.Security.Claims;
using API.Household.Models;
using API.Household.Models.Invitation;
using API.identity;
using API.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Household;

public static class HouseholdEndpoints
{
    public static WebApplication AddHouseholdEndpoints(this WebApplication app)
    {
        var household = app.MapGroup("/household").WithOpenApi().WithTags("Household");

        household.MapPost("/", async ([FromBody]CreateHouseholdDto household, HouseholdService householdService, ClaimsPrincipal principal) =>
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await householdService.CreateHousehold(household, userId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
        
            return Results.Created();
        }).RequireAuthorization(Roles.Member);

        household.MapGet("/", async (ClaimsPrincipal principal, HouseholdService householdService) =>
        {
            // Change to household id accept one have to log out.
            var householdId = principal.FindFirst(Claims.Household)!.Value;
            var result = await householdService.GetHouseholdByUserId(householdId);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        });

        household.MapDelete("/", async (ClaimsPrincipal principal) =>
        {   
            // make sure person is the owner.
            // update all users to no have a househould (ie. remove from householdusers.
            await Task.Delay(1);
        }).RequireAuthorization(Roles.Owner);
        
        household.MapGet("/invites", async (ClaimsPrincipal principal, HouseholdService householdService) =>
        {
            // Show current incomming invitations
            var list = await householdService.Invites(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return list;
        }).RequireAuthorization(Roles.Member);
        
        household.MapGet("/members", async (ClaimsPrincipal principal, HouseholdService householdService) =>
        {
            // Show current incomming invitations
            var result = await householdService.HouseholdMembers(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization(Roles.Member);
        
        household.MapDelete("/members", async (ClaimsPrincipal principal) =>
        {
            // ensure person is in the househould 
            
            // remove household from claims and from householdusers (maybe not use claims....)
            
            await Task.Delay((1));
        }).RequireAuthorization(Roles.Owner);
        
        household.MapPost("/invite", async ([FromBody]CreateInvitationDto invitation, ClaimsPrincipal principal, HouseholdService householdService) =>
        {
            var result = await householdService.InviteUserToHousehold(invitation,
                principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            
            return Results.Created();
        }).RequireAuthorization(Roles.Member);
        
        household.MapPost("/invite/answer", async ([FromBody]InvitationAnswerDto answer, ClaimsPrincipal principal, HouseholdService householdService) =>
        {
            var result = await householdService.AnswerInvitation(answer, principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok();
        }).RequireAuthorization(Roles.Member);

        return app;
    }
}