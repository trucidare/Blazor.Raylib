using Blazor.Raylib.Simple.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class RaylibLogo : ComponentBase
{ 
    [Inject]
    public required ResourceService ResourceService { get; set; }
    
    const int screenWidth = 800;
    const int screenHeight = 450;
    private Texture2D texture;
    private async void Init()
    {
        
        InitWindow(screenWidth, screenHeight, "raylib [textures] example - texture loading and drawing");


        texture = await ResourceService.LoadResourceFromUri<Texture2D>("resources/raylib_logo.png", (e) =>
        {
            var image = LoadImageFromMemory(".png",e);
            return LoadTextureFromImage(image);
        }); 

    }
    // Main game loop
    private async Task Render(float delta)
    { 
        // Update
        //----------------------------------------------------------------------------------
        // TODO: Update your variables here
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

        ClearBackground(Color.White);

        DrawTexture(texture, screenWidth/2 - texture.Width/2, screenHeight/2 - texture.Height/2, Color.White);

        DrawText("this IS a texture!", 360, 370, 10, Color.Gray);

        EndDrawing();

        await Task.CompletedTask;
    }
}