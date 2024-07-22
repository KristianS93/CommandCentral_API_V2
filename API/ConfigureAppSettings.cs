namespace API;

public static class ConfigureAppSettings
{
    public static WebApplicationBuilder ConfigureEnvironment(this WebApplicationBuilder builder)
    {
        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
        switch (builder.Environment.EnvironmentName)
        {
            case "Docker":
                builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: false, reloadOnChange: true);
                // Do somthing with the key for the token
                // var key = Environment.GetEnvironmentVariable("JetSettings:Key");
                Console.WriteLine("Docker loaded!!!");
                break;
            default:
                builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false,
                    reloadOnChange: true);
                // builder.Configuration.AddUserSecrets<Program>();
                Console.WriteLine("Development loaded!!!");
                break;
        }
        // builder.Configuration.AddJsonFile("appsettings.JwtSettings.json", optional: false, reloadOnChange: true);

        return builder;
    }
}