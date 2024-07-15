using System.Text;
using API.Identity;
using API.identity.Models;
using API.SharedAPI.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.identity;

public static class RegisterIdentityServices
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Connection string
        var connectionString = configuration.GetConnectionString("Postgres");
        ArgumentNullException.ThrowIfNullOrEmpty(connectionString);
        services.AddDbContext<AuthDbContext>(options => { options.UseNpgsql(connectionString).UseLowerCaseNamingConvention(); });
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
        

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        
        services.AddIdentity<CCAIdentity, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
        });
        
        // Identity services

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });
        
        
        
        // Authorization

        // services.AddAuthorizationBuilder().AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
            options.AddPolicy(Roles.Owner, policy => policy.RequireRole(Roles.Admin, Roles.Owner));
            options.AddPolicy(Roles.Member, policy => policy.RequireRole(Roles.Admin, Roles.Owner, Roles.Member));
        });
        // DI
        
        // Token configuration
        
        // Authorization configuration


        services.AddDataProtection();

        services.AddTransient<AuthService>();
        services.AddTransient<UserService>();
        
        return services;
    }
}