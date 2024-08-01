using Microsoft.AspNetCore.Components;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class BasicWindow : ComponentBase
{
    private void Init()
    {
        const int screenWidth = 800;
        const int screenHeight = 450;
        
        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
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

}