using System.Numerics;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class ThreeDPicking : ComponentBase
{
    const int screenWidth = 800;
    const int screenHeight = 450;
    private Camera3D camera;
    Ray ray ;                    // Picking line ray
    RayCollision collision ;
    Vector3 cubePosition = new( 0.0f, 1.0f, 0.0f );
    Vector3 cubeSize =  new ( 2.0f, 2.0f, 2.0f );
    private void Init()
    {
        
        InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d picking");

        // Define the camera to look into our 3d world
        camera.Position = new Vector3 ( 10.0f, 10.0f, 10.0f ); // Camera position
        camera.Target = new Vector3( 0.0f, 0.0f, 0.0f );      // Camera looking at point
        camera.Up = new Vector3 ( 0.0f, 1.0f, 0.0f );          // Camera up vector (rotation towards target)
        camera.FovY = 45.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;             // Camera projection type

   

      
    }
    
    // Main game loop
    private async Task Render(float delta)
    {
         // Update
        //----------------------------------------------------------------------------------
        if (IsCursorHidden()) UpdateCamera(ref camera, CameraMode.FirstPerson);

        // Toggle camera controls
        if (IsMouseButtonPressed(MouseButton.Right))
        {
            if (IsCursorHidden()) EnableCursor();
            else DisableCursor();
        }

        if (IsMouseButtonPressed(MouseButton.Left))
        {
            if (!collision.Hit)
            {
                ray = GetMouseRay(GetMousePosition(), camera);

                // Check collision between ray and box
                collision = GetRayCollisionBox(ray,
                            new BoundingBox(new Vector3(cubePosition.X - cubeSize.X/2, cubePosition.Y - cubeSize.Y/2, cubePosition.Z - cubeSize.Z/2), 
                                new Vector3( cubePosition.X + cubeSize.X/2, cubePosition.Y + cubeSize.Y/2, cubePosition.Z + cubeSize.Z/2)));
            }
            else collision.Hit = false;
        }
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            BeginMode3D(camera);

                if (collision.Hit)
                {
                    DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.Red);
                    DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.Maroon);

                    DrawCubeWires(cubePosition, cubeSize.X + 0.2f, cubeSize.Y + 0.2f, cubeSize.Z + 0.2f, Color.Green);
                }
                else
                {
                    DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.Gray);
                    DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, Color.DarkGray);
                }

                DrawRay(ray, Color.Maroon);
                DrawGrid(10, 1.0f);

            EndMode3D();

            DrawText("Try clicking on the box with your mouse!", 240, 10, 20, Color.DarkGray);

            if (collision.Hit) DrawText("BOX SELECTED", (screenWidth - MeasureText("BOX SELECTED", 30)) / 2, (int)(screenHeight * 0.1f), 30, Color.Green);

            DrawText("Right click mouse to toggle camera controls", 10, 430, 10, Color.Gray);

            DrawFPS(10, 10);

        EndDrawing();
        
        await Task.CompletedTask;
    }
}