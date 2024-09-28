using System.Numerics;
using Microsoft.AspNetCore.Components;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;
using Raylib_cs;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class ThreeDFirstPerson : IDisposable
{
    private const int MaxColumns = 20;
    private const int ScreenWidth = 800;
    private const int ScreenHeight = 450;
    readonly float[] _heights = new float[MaxColumns];
    readonly Vector3[] _positions = new Vector3[MaxColumns];
    readonly Color[] _colors = new Color[MaxColumns];
    private Camera3D _camera;
    CameraMode _cameraMode = CameraMode.FirstPerson;
    
    private void Init()
    {
        InitWindow(ScreenWidth, ScreenHeight, "raylib [core] example - 3d camera first person");

        // Define the camera to look into our 3d world (position, target, up vector)
        _camera.Position = new Vector3( 0.0f, 2.0f, 4.0f );    // Camera position
        _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );      // Camera looking at point
        _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f );          // Camera up vector (rotation towards target)
        _camera.FovY = 60.0f;                                // Camera field-of-view Y
        _camera.Projection = CameraProjection.Perspective;             // Camera projection type

        

        // Generates some random columns
        for (int i = 0; i < MaxColumns; i++)
        {
            _heights[i] = (float)GetRandomValue(1, 12);
            _positions[i] = new Vector3( (float)GetRandomValue(-15, 15), _heights[i]/2.0f, (float)GetRandomValue(-15, 15) );
            _colors[i] = new Color( GetRandomValue(20, 255), GetRandomValue(10, 55), 30, 255 );
        }

        DisableCursor();    
        OnResize((ScreenWidth, ScreenHeight));
    }
    
    // Main game loop
    private async void Render(float delta)
    {
        // Update
        //----------------------------------------------------------------------------------
        // Switch camera mode
        if (IsKeyPressed(KeyboardKey.One))
        {
            _cameraMode = CameraMode.Free;
            _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        if (IsKeyPressed(KeyboardKey.Two))
        {
            _cameraMode = CameraMode.FirstPerson;
            _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        if (IsKeyPressed(KeyboardKey.Three))
        {
            _cameraMode = CameraMode.ThirdPerson;
            _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        if (IsKeyPressed(KeyboardKey.Four))
        {
            _cameraMode = CameraMode.Orbital;
            _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f ); // Reset roll
        }

        // Switch camera projection
        if (IsKeyPressed(KeyboardKey.P))
        {
            if (_camera.Projection == CameraProjection.Perspective)
            {
                // Create isometric view
                _cameraMode = CameraMode.ThirdPerson;
                // Note: The target distance is related to the render distance in the orthographic projection
                _camera.Position = new Vector3( 0.0f, 2.0f, -100.0f );
                _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );
                _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f );
                _camera.Projection = CameraProjection.Orthographic;
                _camera.FovY = 20.0f; // near plane width in CAMERA_ORTHOGRAPHIC
                CameraYaw(ref _camera, -135 * DEG2RAD, true);
                CameraPitch(ref _camera, -45 * DEG2RAD, true, true, false);
            }
            else if (_camera.Projection == CameraProjection.Orthographic)
            {
                // Reset to default view
                _cameraMode = CameraMode.ThirdPerson;
                _camera.Position = new Vector3( 0.0f, 2.0f, 10.0f );
                _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );
                _camera.Up = new Vector3( 0.0f, 1.0f, 0.0f );
                _camera.Projection = CameraProjection.Perspective;
                _camera.FovY = 60.0f;
            }
        }

        UpdateCamera(ref _camera, _cameraMode);                  // Update camera
        
        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

            ClearBackground(Color.White);

            BeginMode3D(_camera);

                DrawPlane(new Vector3( 0.0f, 0.0f, 0.0f ), new Vector2( 32.0f, 32.0f ), Color.LightGray); // Draw ground
                DrawCube(new Vector3( -16.0f, 2.5f, 0.0f ), 1.0f, 5.0f, 32.0f, Color.Blue);     // Draw a blue wall
                DrawCube(new Vector3( 16.0f, 2.5f, 0.0f ), 1.0f, 5.0f, 32.0f, Color.Lime);      // Draw a green wall
                DrawCube(new Vector3( 0.0f, 2.5f, 16.0f ), 32.0f, 5.0f, 1.0f, Color.Gold);      // Draw a yellow wall

                // Draw some cubes around
                for (int i = 0; i < MaxColumns; i++)
                {
                    DrawCube(_positions[i], 2.0f, _heights[i], 2.0f, _colors[i]);
                    DrawCubeWires(_positions[i], 2.0f, _heights[i], 2.0f, Color.Maroon);
                }

                // Draw player cube
                if (_cameraMode == CameraMode.ThirdPerson)
                {
                    DrawCube(_camera.Target, 0.5f, 0.5f, 0.5f, Color.Purple);
                    DrawCubeWires(_camera.Target, 0.5f, 0.5f, 0.5f, Color.DarkPurple);
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
            DrawText(string.Format("- Mode: {0}", (_cameraMode == CameraMode.Free) ? "FREE" :
                                              (_cameraMode == CameraMode.FirstPerson) ? "FIRST_PERSON" :
                                              (_cameraMode == CameraMode.ThirdPerson) ? "THIRD_PERSON" :
                                              (_cameraMode == CameraMode.Orbital) ? "ORBITAL" : "CUSTOM"), 610, 30, 10, Color.Black);
            DrawText(string.Format("- Projection: {0}", (_camera.Projection == CameraProjection.Perspective) ? "PERSPECTIVE" :
                                                    (_camera.Projection == CameraProjection.Orthographic) ? "ORTHOGRAPHIC" : "CUSTOM"), 610, 45, 10, Color.Black);
            DrawText(string.Format("- Position: ({0}, {1}, {2})", _camera.Position.X, _camera.Position.Y, _camera.Position.Z), 610, 60, 10, Color.Black);
            DrawText(string.Format("- Target: ({0}, {1}, {2})", _camera.Target.X, _camera.Target.Y, _camera.Target.Z), 610, 75, 10, Color.Black);
            DrawText(string.Format("- Up: ({0}, {1}, {2})", _camera.Up.X, _camera.Up.Y, _camera.Up.Z), 610, 90, 10, Color.Black);

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