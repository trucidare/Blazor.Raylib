using Microsoft.AspNetCore.Components;

namespace Blazor.Raylib.Simple.Components;

public partial class Window : ComponentBase
{
    [Parameter]
    public string? Class { get; set; }
    
    [Parameter]
    public string? Title { get; set; }
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}