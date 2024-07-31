namespace Blazor.Raylib.Simple.Services;

public static class RegisterServices
{
    public static void AddRaylibServices(this IServiceCollection services)
    {
        services.AddSingleton<ResourceService>();
    }
}