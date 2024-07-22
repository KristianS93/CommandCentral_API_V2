using API.identity.Models;
using API.SharedAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Identity;

public class AuthDbContext : IdentityDbContext<CCAIdentity>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // builder.ApplyConfiguration(new HouseholdConfiguration()).ApplyConfiguration(new HouseholdUserRelationConfiguration());
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}