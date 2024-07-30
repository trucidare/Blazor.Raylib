using System.Numerics;
using System.Runtime.InteropServices;
using Blazor.Raylib.Simple.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages;

public partial class Home
{
    [Inject]
    public required ResourceService ResourceService { get; set; }   
    
    private Raylib_cs.Camera3D _camera;
    private readonly Vector3 _cubePosition = Vector3.Zero;
    private Texture2D _texture;

    private async void Init()
    {
        InitWindow(1920, 1080, "Raylib");

        _camera = new Camera3D
        {
            Position = new Vector3(10.0f, 10.0f, 10.0f),
            Target = Vector3.Zero,
            Up = new Vector3(0.0f, 1.0f, 0.0f),
            FovY = 45.0f,
            Projection = CameraProjection.Perspective
        };

        DisableCursor();
        ToggleFullscreen();
        #if RELEASE
        SetTargetFPS(60);
        #endif

        // Santas little helper -> no VFS for blazor 
        _texture =  await ResourceService.LoadResourceFromUri<Texture2D>("resources/skybox.png", (b) =>
        {
            var image = LoadImageFromMemory(".png", b);
            return LoadTextureFromImage(image);
        });
    }

    private async Task Render(float delta)
    {

        DrawFPS(20, 20);
        UpdateCamera(ref _camera, CameraMode.Free);

        if (IsKeyPressed(KeyboardKey.Z))
            _camera.Target = Vector3.Zero;

        BeginDrawing();
        ClearBackground(Color.White);

        BeginMode3D(_camera);
        DrawCube(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Red);
        DrawCubeWires(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Maroon);
        DrawGrid(25, 1.0f);

        EndMode3D();
        EndDrawing();

        await Task.CompletedTask;
    }
}