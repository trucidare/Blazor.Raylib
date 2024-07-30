using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class BasicScreenManager : ComponentBase
{
    private enum GameScreen
    {
        LOGO = 0, 
        TITLE, 
        GAMEPLAY, 
        ENDING
    };

    private GameScreen currentScreen;
    private const int screenWidth = 800;
    private const int screenHeight = 450;
    private int framesCounter = 0;  
    
    private void Init()
    {
        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic screen manager");
        currentScreen = GameScreen.LOGO;
    }
    
    // Main game loop
    private async Task Render(float delta)
    {

                // Update
        //----------------------------------------------------------------------------------
        switch(currentScreen)
        {
            case GameScreen.LOGO:
            {
                // TODO: Update LOGO screen variables here!

                framesCounter++;    // Count frames

                // Wait for 2 seconds (120 frames) before jumping to TITLE screen
                if (framesCounter > 120)
                {
                    currentScreen = GameScreen.TITLE;
                }
            } break;
            case GameScreen.TITLE:
            {
                // TODO: Update TITLE screen variables here!

                // Press enter to change to GAMEPLAY screen
                if (IsKeyPressed(KeyboardKey.Enter) || IsGestureDetected(Gesture.Tap))
                {
                    currentScreen =  GameScreen.GAMEPLAY;
                }
            } break;
            case GameScreen.GAMEPLAY:
            {
                // TODO: Update GAMEPLAY screen variables here!

                // Press enter to change to ENDING screen
                if (IsKeyPressed(KeyboardKey.Enter) || IsGestureDetected(Gesture.Tap))
                {
                    currentScreen = GameScreen.ENDING;
                }
            } break;
            case GameScreen.ENDING:
            {
                // TODO: Update ENDING screen variables here!

                // Press enter to return to TITLE screen
                if (IsKeyPressed(KeyboardKey.Enter) || IsGestureDetected(Gesture.Tap))
                {
                    currentScreen = GameScreen.TITLE;
                }
            } break;
            default: break;
        }
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            switch(currentScreen)
            {
                case GameScreen.LOGO:
                {
                    // TODO: Draw LOGO screen here!
                    DrawText("LOGO SCREEN", 20, 20, 40, Color.LightGray);
                    DrawText("WAIT for 2 SECONDS...", 290, 220, 20, Color.Gray);

                } break;
                case GameScreen.TITLE:
                {
                    // TODO: Draw TITLE screen here!
                    DrawRectangle(0, 0, screenWidth, screenHeight, Color.Green);
                    DrawText("TITLE SCREEN", 20, 20, 40, Color.DarkGreen);
                    DrawText("PRESS ENTER or TAP to JUMP to GAMEPLAY SCREEN", 120, 220, 20, Color.DarkGreen);

                } break;
                case GameScreen.GAMEPLAY:
                {
                    // TODO: Draw GAMEPLAY screen here!
                    DrawRectangle(0, 0, screenWidth, screenHeight, Color.Purple);
                    DrawText("GAMEPLAY SCREEN", 20, 20, 40, Color.Maroon);
                    DrawText("PRESS ENTER or TAP to JUMP to ENDING SCREEN", 130, 220, 20, Color.Maroon);

                } break;
                case GameScreen.ENDING:
                {
                    // TODO: Draw ENDING screen here!
                    DrawRectangle(0, 0, screenWidth, screenHeight, Color.Blue);
                    DrawText("ENDING SCREEN", 20, 20, 40, Color.DarkBlue);
                    DrawText("PRESS ENTER or TAP to RETURN to TITLE SCREEN", 120, 220, 20, Color.DarkBlue);

                } break;
                default: break;
            }

        EndDrawing();

        await Task.CompletedTask;
    }
}