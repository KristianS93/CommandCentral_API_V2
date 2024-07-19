using System.Security.Claims;
using API.GroceryList;
using API.GroceryList.Models;
using API.Household.Models;
using API.Household.Models.Invitation;
using API.identity;
using API.Identity;
using API.identity.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Household;

public class HouseholdService
{
    private readonly UserManager<CCAIdentity> _userManager;
    private readonly ApiDbContext _apiDbContext;
    private readonly GroceryListService _groceryListService;
    
    public HouseholdService(UserManager<CCAIdentity> userManager, ApiDbContext apiDbContext, GroceryListService groceryListService)
    {
        _userManager = userManager;
        _apiDbContext = apiDbContext;
        _groceryListService = groceryListService;
    }

    public async Task<Result> CreateHousehold(CreateHouseholdDto householdDto, string userId)
    {
        // check household does not exist
        var household = await _apiDbContext.HouseholdUsers.Where(e => e.UserId == userId).FirstOrDefaultAsync();
        if (household is not null)
        {
            return Result.Fail("Already have a household");
        }
        
        var houseHoldId = Guid.NewGuid().ToString();
        var newHousehold = new HouseholdModel { Name = householdDto.Name, HouseholdId = houseHoldId};
        
        await _apiDbContext.Households.AddAsync(newHousehold);
        var relation = new HouseholdUsersModel
            { HouseholdId = houseHoldId, UserId = userId, Role = Roles.Owner };
        await _apiDbContext.SaveChangesAsync();
        
        await _apiDbContext.HouseholdUsers.AddAsync(relation);
        
        // get current role check it is less than owner
        var user = await _userManager.FindByIdAsync(userId);
        var currentRole = (await _userManager.GetRolesAsync(user!)).First();
        bool changeRole = currentRole switch
        {
            "Member" => true,
            _ => false
        };
        if (changeRole)
        {
            // remove current role
            await _userManager.RemoveFromRoleAsync(user!, currentRole);
            await _userManager.AddToRoleAsync(user!, Roles.Owner);
        }
        
        // Consider not having claims.
        await _userManager.RemoveClaimAsync(user!, new Claim(Claims.Household, Claims.HouseholdDefault));
        await _userManager.AddClaimAsync(user!, new Claim(Claims.Household, houseHoldId));
        
        // Create groceryList
        await _apiDbContext.GroceryLists.AddAsync(new GroceryListModel
        {
            HousehouldId = houseHoldId,
        });
        await _apiDbContext.SaveChangesAsync();
        
        return Result.Ok();
    }

    public async Task<Result<HouseholdDto>> GetHouseholdByUserId(string householdId)
    {
        // change such that it uses userid
        if (householdId == "None")
        {
            return Result.Fail(new Error("No household").WithMetadata("HouseholdId", householdId).CausedBy("Claim not found."));
        }

        var result = await _apiDbContext.Households.FindAsync(householdId);
        if (result is null)
        {
            return Result.Fail(new Error("Error Loading household").WithMetadata("HouseholdId", householdId).CausedBy("Claim not found."));
        }

        // var household = await _apiDbContext.Households.FindAsync(result.HouseholdId);

        return Result.Ok(new HouseholdDto(result.HouseholdId, result.Name, result.LastModified, result.CreatedAt.Date));
    }

    public async Task<Result<List<HouseholdMemberDto>>> HouseholdMembers(string userId)
    {
        // check user has household
        var userHasHousehold = await _apiDbContext.HouseholdUsers.FindAsync(userId);
        if (userHasHousehold is null)
        {
            return Result.Fail(new Error("User does not have a household.").WithMetadata("Household", null)
                .CausedBy("User does not have a household"));
        }
        // return list of households with that id.
        var listOfuserIds = await _apiDbContext.HouseholdUsers.Where(m => m.HouseholdId == userHasHousehold.HouseholdId)
            .ToListAsync();
        var listOfMembers = new List<HouseholdMemberDto>();
        foreach (var user in listOfuserIds)
        {
            var getName = await _userManager.FindByIdAsync(user.UserId);
            listOfMembers.Add(new HouseholdMemberDto(user.UserId, getName!.FirstName, user.Role));
        }

        return Result.Ok(listOfMembers);
    }

    #region Invitations
    public async Task<Result> InviteUserToHousehold(CreateInvitationDto invitation, string userId)
    {
        if (invitation.Email.IsNullOrEmpty())
        {
            return Result.Fail(new Error("Could not invite user").WithMetadata("UserEmail", invitation.Email).CausedBy("Email Error"));
        }
        
        // make sure inviter has a household:
        var household = await _apiDbContext.HouseholdUsers.FindAsync(userId);
        if (household is null || household.Role == Roles.Member)
        {
            return Result.Fail(new Error("You dont have a household to invite to.").WithMetadata("HouseholdPermission", invitation.Email).CausedBy("Household"));

        }
        
        var userToInvite = await _userManager.FindByEmailAsync(invitation.Email);
        if (userToInvite is null)
        {
            return Result.Fail(new Error("Could not invite user").WithMetadata("UserEmail", invitation.Email).CausedBy("Email Error"));
        }
        // make sure person does not exist in Householdusers
        var hasHousehold = await _apiDbContext.HouseholdUsers.Where(p => p.UserId == userToInvite.Id).ToArrayAsync();
        if (hasHousehold.Any())
        {
            return Result.Fail(new Error("Could not invite user").WithMetadata("UserEmail", invitation.Email).CausedBy("Email Error"));
        }
        
        // Ok, create invite
        var x = await _apiDbContext.Invitations.AddAsync(new InvitationModel
            { InviteeUserId = userToInvite.Id,InviterUserId = userId, HouseholdId = household.HouseholdId });
        await _apiDbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> AnswerInvitation(InvitationAnswerDto answer, string userId)
    {
        //Todo: need to handle the update on the userclaims like in the create household.
        var invitation = await _apiDbContext.Invitations.FindAsync(answer.InvitationId);
        if (invitation is null)
        {
            return Result.Fail(new Error("Invitation does not exist.").WithMetadata("Invitation", answer.InvitationId)
                .CausedBy("Invitation missing."));
        }
        if (answer.Answer)
        {
            await _apiDbContext.HouseholdUsers.AddAsync(new HouseholdUsersModel
            {
                HouseholdId = invitation.HouseholdId,
                Role = Roles.Member,
                UserId = userId,
            });
        }
        _apiDbContext.Invitations.Remove(invitation);
        await _apiDbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<List<InviteDto>> Invites(string userId)
    {
        var getInvites = await _apiDbContext.Invitations.Where(u => u.InviteeUserId == userId).ToListAsync();
        if (getInvites.IsNullOrEmpty())
        {
            return new List<InviteDto>();
        }

        var invites = new List<InviteDto>();
        // Invites exist get household names and inviter emails 
        foreach (var invite in getInvites)
        {
            var inviterDetails = await _userManager.FindByIdAsync(invite.InviterUserId);
            var householdDetails = await _apiDbContext.Households.FindAsync(invite.HouseholdId);
            invites.Add(new InviteDto(inviterDetails!.Email!, householdDetails!.Name));
        }

        return invites;
    }
    #endregion
    
    
    
}