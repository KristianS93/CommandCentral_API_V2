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
                break;
            default:
                builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false,
                    reloadOnChange: true);
                break;
        }

        return builder;
    }
}