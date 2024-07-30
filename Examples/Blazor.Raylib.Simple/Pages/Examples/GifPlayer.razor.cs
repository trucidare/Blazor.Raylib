using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class GifPlayer : ComponentBase
{
    [Inject]
    public required ResourceService ResourceService { get; set; } 
    
    private const int MAX_FRAME_DELAY = 20;
    private const int MIN_FRAME_DELAY = 1;
    const int screenWidth = 800;
    const int screenHeight = 450;

    int animFrames = 0;

    Image imScarfyAnim;
    Texture2D texScarfyAnim ;

    int nextFrameDataOffset = 0;  // Current byte offset to next frame in image.data

    int currentAnimFrame = 0;       // Current animation frame to load and draw
    int frameDelay = 8;             // Frame delay to switch between animation frames
    int frameCounter = 0;           // General frames counter
    
    private async void Init()
    {
        InitWindow(screenWidth, screenHeight, "raylib [textures] example - gif playing");

        imScarfyAnim = await ResourceService.LoadResourceFromUri<Image>("resources/scarfy_run.gif", (e) => RaylibExtensions.LoadImageAnimFromMemory("resources/scarfy_run.gif", e, out animFrames));
        
        texScarfyAnim = LoadTextureFromImage(imScarfyAnim);
    }
    
    // Main game loop
    private async Task Render(float delta)
    {
        // Update
        //----------------------------------------------------------------------------------
        frameCounter++;
        if (frameCounter >= frameDelay)
        {
            // Move to next frame
            // NOTE: If final frame is reached we return to first frame
            currentAnimFrame++;
            if (currentAnimFrame >= animFrames) currentAnimFrame = 0;

            // Get memory offset position for next frame data in image.data
            nextFrameDataOffset = imScarfyAnim.Width*imScarfyAnim.Height*4*currentAnimFrame;

            // Update GPU texture data with next frame image data
            // WARNING: Data size (frame size) and pixel format must match already created texture
            unsafe
            {
                UpdateTexture(texScarfyAnim, ((char *)imScarfyAnim.Data) + nextFrameDataOffset);
            }

            frameCounter = 0;
        }

        // Control frames delay
        if (IsKeyPressed(KeyboardKey.Right)) frameDelay++;
        else if (IsKeyPressed(KeyboardKey.Left)) frameDelay--;

        if (frameDelay > MAX_FRAME_DELAY) frameDelay = MAX_FRAME_DELAY;
        else if (frameDelay < MIN_FRAME_DELAY) frameDelay = MIN_FRAME_DELAY;
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            DrawText(string.Format("TOTAL GIF FRAMES:  {0}", animFrames), 50, 30, 20, Color.LightGray);
            DrawText(string.Format("CURRENT FRAME: {0}", currentAnimFrame), 50, 60, 20, Color.Gray);
            DrawText(string.Format("CURRENT FRAME IMAGE.DATA OFFSET: {0}", nextFrameDataOffset), 50, 90, 20, Color.Gray);

            DrawText("FRAMES DELAY: ", 100, 305, 10, Color.DarkGray);
            DrawText(string.Format("{0} frames", frameDelay), 620, 305, 10, Color.DarkGray);
            DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 350, 10, Color.DarkGray);

            for (int i = 0; i < MAX_FRAME_DELAY; i++)
            {
                if (i < frameDelay) DrawRectangle(190 + 21*i, 300, 20, 20, Color.Red);
                DrawRectangleLines(190 + 21*i, 300, 20, 20, Color.Maroon);
            }

            DrawTexture(texScarfyAnim, GetScreenWidth()/2 - texScarfyAnim.Width/2, 140, Color.White);

            DrawText("(c) Scarfy sprite by Eiden Marsal", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

        EndDrawing();
    }
}