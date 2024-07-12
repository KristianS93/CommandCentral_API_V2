using API;

var builder = WebApplication.CreateBuilder(args);

// Configure environment
builder.ConfigureEnvironment();

// Add services to the container.
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
    app.UseCors("docker");
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
    {
        var forecast = "hello world";
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

