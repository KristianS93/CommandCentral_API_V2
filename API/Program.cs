using System.Security.Claims;
using API;
using API.GroceryList;
using API.Household;
using API.identity;
using API.Identity;
using API.SharedAPI;
using API.SharedAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configure environment
builder.ConfigureEnvironment();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddSharedServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddHouseholdServices();
builder.Services.AddGroceryListServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Production and Development cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("development", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader();
    });
    options.AddPolicy("docker", policyBuilder =>
    {
        policyBuilder.WithOrigins("devspace.dk").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("development");
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("development");
}

app.UseHttpsRedirection();

await app.AddIdentityApp();
await app.AddSharedApp();
await app.AddRawTables();

// app.AddIdentityEndpoints();
app.AddHouseholdEndpoints();
app.AddGroceryListEndpoints();

app.MapGet("/xd", (ClaimsPrincipal principle) =>
{
    // foreach (var claim in principle.Claims)
    // {
    //     Console.WriteLine(claim.ToString() + " " + claim.Value);
    // }
    var userId = principle.FindFirst(Claims.Household)!.Value;
    Console.WriteLine(userId);
    return Results.Ok($"Hello {principle.Identity!.Name}");
}).RequireAuthorization();

app.Run();

