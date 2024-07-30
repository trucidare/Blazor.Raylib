using System.Numerics;
using Microsoft.AspNetCore.Components;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;
using Raylib_cs;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class ThreeDFirstPerson : ComponentBase
{
    private const int MAX_COLUMNS = 20;
    private const int screenWidth = 800;
    private const int screenHeight = 450;
    float[] heights = new float[MAX_COLUMNS];
    Vector3[] positions = new Vector3[MAX_COLUMNS];
    Color[] colors = new Color[MAX_COLUMNS];
    private Camera3D camera;
    CameraMode cameraMode = CameraMode.FirstPerson;
    
    private void Init()
    {
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d camera first person");

        // Define the camera to look into our 3d world (position, target, up vector)
        camera.Position = new Vector3( 0.0f, 2.0f, 4.0f );    // Camera position
        camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );      // Camera looking at point
        camera.Up = new Vector3( 0.0f, 1.0f, 0.0f );          // Camera up vector (rotation towards target)
        camera.FovY = 60.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;             // Camera projection type

        

        // Generates some random columns
      

        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            heights[i] = (float)GetRandomValue(1, 12);
            positions[i] = new Vector3( (float)GetRandomValue(-15, 15), heights[i]/2.0f, (float)GetRandomValue(-15, 15) );
            colors[i] = new Color( GetRandomValue(20, 255), GetRandomValue(10, 55), 30, 255 );
        }

        DisableCursor();    
    }
    
    // Main game loop
    private async Task Render(float delta)
    {
        // Update
        //----------------------------------------------------------------------------------
        // Switch camera mode
        if (IsKeyPressed(KeyboardKey.One))
        {
            cameraMode = CameraMode.Free;
            camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        if (IsKeyPressed(KeyboardKey.Two))
        {
            cameraMode = CameraMode.FirstPerson;
            camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        if (IsKeyPressed(KeyboardKey.Three))
        {
            cameraMode = CameraMode.ThirdPerson;
            camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        if (IsKeyPressed(KeyboardKey.Four))
        {
            cameraMode = CameraMode.Orbital;
            camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        // Switch camera projection
        if (IsKeyPressed(KeyboardKey.P))
        {
            if (camera.Projection == CameraProjection.Perspective)
            {
                // Create isometric view
                cameraMode = CameraMode.ThirdPerson;
                // Note: The target distance is related to the render distance in the orthographic projection
                camera.Position = new Vector3( 0.0f, 2.0f, -100.0f );
                camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );
                camera.Up = new Vector3( 0.0f, 1.0f, 0.0f );
                camera.Projection = CameraProjection.Orthographic;
                camera.FovY = 20.0f; // near plane width in CAMERA_ORTHOGRAPHIC
                CameraYaw(ref camera, -135 * DEG2RAD, true);
                CameraPitch(ref camera, -45 * DEG2RAD, true, true, false);
            }
            else if (camera.Projection == CameraProjection.Orthographic)
            {
                // Reset to default view
                cameraMode = CameraMode.ThirdPerson;
                camera.Position = new Vector3( 0.0f, 2.0f, 10.0f );
                camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );
                camera.Up = new Vector3( 0.0f, 1.0f, 0.0f );
                camera.Projection = CameraProjection.Perspective;
                camera.FovY = 60.0f;
            }
        }

        // Update camera computes movement internally depending on the camera mode
        // Some default standard keyboard/mouse inputs are hardcoded to simplify use
        // For advance camera controls, it's reecommended to compute camera movement manually
        UpdateCamera(ref camera, cameraMode);                  // Update camera

/*
        // Camera PRO usage example (EXPERIMENTAL)
        // This new camera function allows custom movement/rotation values to be directly provided
        // as input parameters, with this approach, rcamera module is internally independent of raylib inputs
        UpdateCameraPro(&camera,
            new Vector3(
                (IsKeyDown(KEY_W) || IsKeyDown(KEY_UP))*0.1f -      // Move forward-backward
                (IsKeyDown(KEY_S) || IsKeyDown(KEY_DOWN))*0.1f,    
                (IsKeyDown(KEY_D) || IsKeyDown(KEY_RIGHT))*0.1f -   // Move right-left
                (IsKeyDown(KEY_A) || IsKeyDown(KEY_LEFT))*0.1f,
                0.0f                                                // Move up-down
            },
            new Vector3(
                GetMouseDelta().x*0.05f,                            // Rotation: yaw
                GetMouseDelta().y*0.05f,                            // Rotation: pitch
                0.0f                                                // Rotation: roll
            },
            GetMouseWheelMove()*2.0f);                              // Move to target (zoom)
*/
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            BeginMode3D(camera);

                DrawPlane(new Vector3( 0.0f, 0.0f, 0.0f ), new Vector2( 32.0f, 32.0f ), Color.LightGray); // Draw ground
                DrawCube(new Vector3( -16.0f, 2.5f, 0.0f ), 1.0f, 5.0f, 32.0f, Color.Blue);     // Draw a blue wall
                DrawCube(new Vector3( 16.0f, 2.5f, 0.0f ), 1.0f, 5.0f, 32.0f, Color.Lime);      // Draw a green wall
                DrawCube(new Vector3( 0.0f, 2.5f, 16.0f ), 32.0f, 5.0f, 1.0f, Color.Gold);      // Draw a yellow wall

                // Draw some cubes around
                for (int i = 0; i < MAX_COLUMNS; i++)
                {
                    DrawCube(positions[i], 2.0f, heights[i], 2.0f, colors[i]);
                    DrawCubeWires(positions[i], 2.0f, heights[i], 2.0f, Color.Maroon);
                }

                // Draw player cube
                if (cameraMode == CameraMode.ThirdPerson)
                {
                    DrawCube(camera.Target, 0.5f, 0.5f, 0.5f, Color.Purple);
                    DrawCubeWires(camera.Target, 0.5f, 0.5f, 0.5f, Color.DarkPurple);
                }

            EndMode3D();

            // Draw info boxes
            DrawRectangle(5, 5, 330, 100, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(5, 5, 330, 100, Color.Blue);

            DrawText("Camera controls:", 15, 15, 10, Color.Black);
            DrawText("- Move keys: W, A, S, D, Space, Left-Ctrl", 15, 30, 10, Color.Black);
            DrawText("- Look around: arrow keys or mouse", 15, 45, 10, Color.Black);
            DrawText("- Camera mode keys: 1, 2, 3, 4", 15, 60, 10, Color.Black);
            DrawText("- Zoom keys: num-plus, num-minus or mouse scroll", 15, 75, 10, Color.Black);
            DrawText("- Camera projection key: P", 15, 90, 10, Color.Black);

            DrawRectangle(600, 5, 195, 100, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(600, 5, 195, 100, Color.Blue);

            DrawText("Camera status:", 610, 15, 10, Color.Black);
            DrawText(string.Format("- Mode: {0}", (cameraMode == CameraMode.Free) ? "FREE" :
                                              (cameraMode == CameraMode.FirstPerson) ? "FIRST_PERSON" :
                                              (cameraMode == CameraMode.ThirdPerson) ? "THIRD_PERSON" :
                                              (cameraMode == CameraMode.Orbital) ? "ORBITAL" : "CUSTOM"), 610, 30, 10, Color.Black);
            DrawText(string.Format("- Projection: {0}", (camera.Projection == CameraProjection.Perspective) ? "PERSPECTIVE" :
                                                    (camera.Projection == CameraProjection.Orthographic) ? "ORTHOGRAPHIC" : "CUSTOM"), 610, 45, 10, Color.Black);
            DrawText(string.Format("- Position: ({0}, {1}, {2})", camera.Position.X, camera.Position.Y, camera.Position.Z), 610, 60, 10, Color.Black);
            DrawText(string.Format("- Target: ({0}, {1}, {2})", camera.Target.X, camera.Target.Y, camera.Target.Z), 610, 75, 10, Color.Black);
            DrawText(string.Format("- Up: ({0}, {1}, {2})", camera.Up.X, camera.Up.Y, camera.Up.Z), 610, 90, 10, Color.Black);

        EndDrawing();
    }
}