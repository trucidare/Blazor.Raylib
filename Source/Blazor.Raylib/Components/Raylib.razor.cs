using Microsoft.JSInterop;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices.JavaScript;
using Blazor.Raylib.Extensions;

namespace Blazor.Raylib.Components;

[SupportedOSPlatform("browser")]
public partial class Raylib : IDisposable
{
    [Inject]
    public required IJSRuntime Runtime { get; set; }
    
    [Parameter]
    public RenderCallback? OnRender { get; set; }
    
    [Parameter]
    public EventCallback<(int Width, int Height)> OnResize { get; set; }
    
    [Parameter]
    public Action? OnInit { get; set; } 
    
    [Parameter]
    public bool UseEmscriptenMainLoop { get; set; }

    [Parameter] 
    public int MaxFPS { get; set; }
    
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }
    
    private readonly string _id = $"canvas";
 
    protected override async Task OnInitializedAsync()
    {
        await JSHost.ImportAsync("Raylib", "../_content/Blazor.Raylib/Components/Raylib.razor.js");
        Init(this, _id);
        InitRaylib();
        ManageRenderLoop();
    }

    private void InitRaylib()
    {
       OnInit?.Invoke();
    }

    private void ManageRenderLoop()
    {
        var maxFps = MaxFPS == 0 ? 1000 : MaxFPS;
        if (OnRender != null)
            if (!UseEmscriptenMainLoop)
                Render(this, _id, maxFps);
            else
                RaylibExtensions.SetMainLoop(OnRender, MaxFPS);
    }

    #region Interop

    [JSImport("raylib.init", "Raylib")]
    public static partial void Init([JSMarshalAs<JSType.Any>] object reference, string id);


    [JSImport("raylib.render", "Raylib")]
    public static partial void Render([JSMarshalAs<JSType.Any>] object reference, string id, int fps);


    [JSExport]
    private static async Task EventAnimationFrame([JSMarshalAs<JSType.Any>] object reference, float timeDelta)
    {
        if (reference is Raylib rl)
            rl.OnRender?.Invoke(timeDelta);

        await Task.CompletedTask;
    }
    
    [JSExport]
    private static async Task ResizeCanvas([JSMarshalAs<JSType.Any>] object reference, int width, int height, int dpr)
    {
        if (reference is Raylib { OnResize.HasDelegate: true } rl)
            await rl.OnResize.InvokeAsync((width, height));

        await Task.CompletedTask;
    }
    
    #endregion

    public void Dispose()
    {
        OnRender = null;
    }
}