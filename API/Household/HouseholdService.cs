using API.Household.Models;
using API.identity;
using API.identity.Models;
using API.SharedAPI.Persistence;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Household;

public class HouseholdService
{
    private readonly UserManager<CCAIdentity> _userManager;
    private readonly ApiDbContext _apiDbContext;
    
    public HouseholdService(UserManager<CCAIdentity> userManager, ApiDbContext apiDbContext)
    {
        _userManager = userManager;
        _apiDbContext = apiDbContext;
    }

    public async Task<Result> CreateHousehold(CreateHouseholdDTO householdDto)
    {
        // check household does not exist
        var household = await _apiDbContext.HouseholdUsers.Where(e => e.UserId == householdDto.userId).FirstOrDefaultAsync();
        if (household is not null)
        {
            return Result.Fail("Already have a household");
        }

        var houseHoldId = Guid.NewGuid().ToString();
        Console.WriteLine("-- New household id " + houseHoldId);
        Console.WriteLine("--- user id "+ householdDto.userId );
        var newHousehold = new HouseholdModel { Name = householdDto.name, HouseholdId = houseHoldId};
        await _apiDbContext.Households.AddAsync(newHousehold);
        await _apiDbContext.SaveChangesAsync();
        var relation = new HouseholdUsersModel
            { HouseholdId = houseHoldId, UserId = householdDto.userId, Role = Roles.Owner };
        await _apiDbContext.HouseholdUsers.AddAsync(relation);
        // get current role check it is less than owner
        var user = await _userManager.FindByIdAsync(householdDto.userId);
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
        
        return Result.Ok();
    }
}