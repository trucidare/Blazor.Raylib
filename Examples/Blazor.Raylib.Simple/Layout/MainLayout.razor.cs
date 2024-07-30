using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Blazor.Raylib.Simple.Layout;

public partial class MainLayout
{
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private void NavigateTo(string url, bool reload = false)
    {
        NavigationManager.NavigateTo(url, reload);
    }

    private string? GetVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version?.ToString();
        return version;
    }
}