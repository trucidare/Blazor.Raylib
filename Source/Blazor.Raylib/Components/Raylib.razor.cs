using Microsoft.JSInterop;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices.JavaScript;

namespace Blazor.Raylib.Components;

[SupportedOSPlatform("browser")]
public partial class Raylib
{
    [Inject]
    public required IJSRuntime Runtime { get; set; }
    
    [Parameter]
    public EventCallback<float> OnRender { get; set; }
    
    [Parameter]
    public Action? OnInit { get; set; } 
    
    private readonly string _id = $"canvas";
 
    protected override async Task OnInitializedAsync()
    {
        await JSHost.ImportAsync("Raylib", "../_content/Blazor.Raylib/Components/Raylib.razor.js");
        Init(_id);
        Render(this, _id);
        InitRaylib();
    }

    private void InitRaylib()
    {
       OnInit?.Invoke();
    }


    [JSImport("raylib.init", "Raylib")]
    public static partial void Init(string id);


    [JSImport("raylib.render", "Raylib")]
    public static partial void Render([JSMarshalAs<JSType.Any>] object reference, string id);


    [JSExport]
    private static async Task EventAnimationFrame([JSMarshalAs<JSType.Any>] object reference, float timeDelta)
    {
        if (reference is Raylib rl)
            await rl.OnRender.InvokeAsync(timeDelta);
    }
}