using System.Numerics;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class TwoDCamera : ComponentBase
{
    const int screenWidth = 800;
    const int screenHeight = 450;
    const int MAX_BUILDINGS  =100;
    
    Rectangle player = new Rectangle(400, 280, 40, 40);
    Rectangle[] buildings =new Rectangle[MAX_BUILDINGS];
    Color[] buildColors = new Color[MAX_BUILDINGS];
    private Camera2D camera = new();
    private void Init()
    {
        InitWindow(screenWidth, screenHeight, "raylib [core] example - 2d camera");
        
        int spacing = 0;

        for (int i = 0; i < MAX_BUILDINGS; i++)
        {
            buildings[i].Width = (float)GetRandomValue(50, 200);
            buildings[i].Height = (float)GetRandomValue(100, 800);
            buildings[i].Y = screenHeight - 130.0f - buildings[i].Height;
            buildings[i].X = -6000.0f + spacing;

            spacing += (int)buildings[i].Width;

            buildColors[i] = new Color(GetRandomValue(200, 240), GetRandomValue(200, 240), GetRandomValue(200, 250),
                255);
        }

       
        camera.Target = new Vector2(player.X + 20.0f, player.Y + 20.0f);
        camera.Offset = new Vector2( screenWidth/2.0f, screenHeight/2.0f );
        camera.Rotation = 0.0f;
        camera.Zoom = 1.0f;

    }
    
    // Main game loop
    private async Task Render(float delta)
    {
        // Update
        //----------------------------------------------------------------------------------
        // Player movement
        if (IsKeyDown(KeyboardKey.Right)) player.X += 2;
        else if (IsKeyDown(KeyboardKey.Left)) player.X -= 2;

        // Camera target follows player
        camera.Target = new Vector2( player.X + 20, player.Y + 20);

        // Camera rotation controls
        if (IsKeyDown(KeyboardKey.A)) camera.Rotation--;
        else if (IsKeyDown(KeyboardKey.S)) camera.Rotation++;

        // Limit camera rotation to 80 degrees (-40 to 40)
        if (camera.Rotation > 40) camera.Rotation = 40;
        else if (camera.Rotation < -40) camera.Rotation = -40;

        // Camera zoom controls
        camera.Zoom += GetMouseWheelMove()*0.05f;

        if (camera.Zoom > 3.0f) camera.Zoom = 3.0f;
        else if (camera.Zoom < 0.1f) camera.Zoom = 0.1f;

        // Camera reset (zoom and rotation)
        if (IsKeyPressed(KeyboardKey.R))
        {
            camera.Zoom = 1.0f;
            camera.Rotation = 0.0f;
        }
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            BeginMode2D(camera);

                DrawRectangle(-6000, 320, 13000, 8000, Color.DarkPurple);

                for (int i = 0; i < MAX_BUILDINGS; i++) DrawRectangleRec(buildings[i], buildColors[i]);

                DrawRectangleRec(player, Color.Red);

                DrawLine((int)camera.Target.X, -screenHeight*10, (int)camera.Target.X, screenHeight*10, Color.Green);
                DrawLine(-screenWidth*10, (int)camera.Target.Y, screenWidth*10, (int)camera.Target.Y, Color.Green);

            EndMode2D();

            DrawText("SCREEN AREA", 640, 10, 20, Color.Red);

            DrawRectangle(0, 0, screenWidth, 5, Color.Red);
            DrawRectangle(0, 5, 5, screenHeight - 10, Color.Red);
            DrawRectangle(screenWidth - 5, 5, 5, screenHeight - 10, Color.Red);
            DrawRectangle(0, screenHeight - 5, screenWidth, 5, Color.Red);

            DrawRectangle( 10, 10, 250, 113, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines( 10, 10, 250, 113, Color.Blue);

            DrawText("Free 2d camera controls:", 20, 20, 10, Color.Black);
            DrawText("- Right/Left to move Offset", 40, 40, 10, Color.DarkGray);
            DrawText("- Mouse Wheel to Zoom in-out", 40, 60, 10, Color.DarkGray);
            DrawText("- A / S to Rotate", 40, 80, 10, Color.DarkGray);
            DrawText("- R to reset Zoom and Rotation", 40, 100, 10, Color.DarkGray);

        EndDrawing();

        await Task.CompletedTask;
    }

}