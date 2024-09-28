using Microsoft.AspNetCore.Components;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class BasicWindow : IDisposable
{
    private void Init()
    {
        const int screenWidth = 800;
        const int screenHeight = 450;
        
        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
        OnResize((screenWidth, screenHeight));
    }
    
    // Main game loop
    private async void Render(float delta)
    {

        // Update
        //----------------------------------------------------------------------------------
        // TODO: Update your variables here
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

        ClearBackground(Color.White);

        DrawText("Congrats! You created your first window!", 190, 200, 20, Color.LightGray);

        EndDrawing();

        await Task.CompletedTask;
    }
    
    private void OnResize((int width, int height) Size)
    {
        SetWindowSize(Size.width, Size.height);
    }
    
    public void Dispose()
    {
        CloseWindow();
    }

}