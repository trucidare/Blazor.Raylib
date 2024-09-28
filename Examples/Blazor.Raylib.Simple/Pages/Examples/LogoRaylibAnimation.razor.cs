using Blazor.Raylib.Extensions;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class LogoRaylibAnimation : IDisposable
{
    
    const int screenWidth = 800;
    const int screenHeight = 450;
    
    int logoPositionX = screenWidth/2 - 128;
    int logoPositionY = screenHeight/2 - 128;

    int framesCounter = 0;
    int lettersCount = 0;

    int topSideRecWidth = 16;
    int leftSideRecHeight = 16;

    int bottomSideRecWidth = 16;
    int rightSideRecHeight = 16;

    int state = 0;                  // Tracking animation states (State Machine)
    float alpha = 1.0f;             // Useful for fading
    
    private void Init()
    {
        InitWindow(screenWidth, screenHeight, "raylib [shapes] example - raylib logo animation");
        OnResize((screenWidth, screenHeight));
    }

    // Main game loop
    private async void Render(float delta)
    {
         // Update
        //----------------------------------------------------------------------------------
        if (state == 0)                 // State 0: Small box blinking
        {
            framesCounter++;

            if (framesCounter == 120)
            {
                state = 1;
                framesCounter = 0;      // Reset counter... will be used later...
            }
        }
        else if (state == 1)            // State 1: Top and left bars growing
        {
            topSideRecWidth += 4;
            leftSideRecHeight += 4;

            if (topSideRecWidth == 256) state = 2;
        }
        else if (state == 2)            // State 2: Bottom and right bars growing
        {
            bottomSideRecWidth += 4;
            rightSideRecHeight += 4;

            if (bottomSideRecWidth == 256) state = 3;
        }
        else if (state == 3)            // State 3: Letters appearing (one by one)
        {
            framesCounter++;

            if (framesCounter %12 == 0 )       // Every 12 frames, one more letter!
            {
                lettersCount++;
                framesCounter = 0;
            }

            if (lettersCount >= 10)     // When all letters have appeared, just fade out everything
            {
                alpha -= 0.02f;

                if (alpha <= 0.0f)
                {
                    alpha = 0.0f;
                    state = 4;
                }
            }
        }
        else if (state == 4)            // State 4: Reset and Replay
        {
            if (IsKeyPressed(KeyboardKey.R))
            {
                framesCounter = 0;
                lettersCount = 0;

                topSideRecWidth = 16;
                leftSideRecHeight = 16;

                bottomSideRecWidth = 16;
                rightSideRecHeight = 16;

                alpha = 1.0f;
                state = 0;          // Return to State 0
            }
        }
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            if (state == 0)
            {
                if ((framesCounter/15)%2 != 0) 
                    DrawRectangle(logoPositionX, logoPositionY, 16, 16, Color.Black);
            }
            else if (state == 1)
            {
                DrawRectangle(logoPositionX, logoPositionY, topSideRecWidth, 16, Color.Black);
                DrawRectangle(logoPositionX, logoPositionY, 16, leftSideRecHeight, Color.Black);
            }
            else if (state == 2)
            {
                DrawRectangle(logoPositionX, logoPositionY, topSideRecWidth, 16, Color.Black);
                DrawRectangle(logoPositionX, logoPositionY, 16, leftSideRecHeight, Color.Black);

                DrawRectangle(logoPositionX + 240, logoPositionY, 16, rightSideRecHeight, Color.Black);
                DrawRectangle(logoPositionX, logoPositionY + 240, bottomSideRecWidth, 16, Color.Black);
            }
            else if (state == 3)
            {
                DrawRectangle(logoPositionX, logoPositionY, topSideRecWidth, 16, Fade(Color.Black, alpha));
                DrawRectangle(logoPositionX, logoPositionY + 16, 16, leftSideRecHeight - 32, Fade(Color.Black, alpha));

                DrawRectangle(logoPositionX + 240, logoPositionY + 16, 16, rightSideRecHeight - 32, Fade(Color.Black, alpha));
                DrawRectangle(logoPositionX, logoPositionY + 240, bottomSideRecWidth, 16, Fade(Color.Black, alpha));

                DrawRectangle(GetScreenWidth()/2 - 112, GetScreenHeight()/2 - 112, 224, 224, Fade(Color.White, alpha));
                
                DrawText(RaylibExtensions.TextSubtext("raylib", 0, lettersCount), GetScreenWidth()/2 - 44, GetScreenHeight()/2 + 48, 50, Fade(Color.Black, alpha));
            }
            else if (state == 4)
            {
                DrawText("[R] REPLAY", 340, 200, 20, Color.Gray);
            }

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