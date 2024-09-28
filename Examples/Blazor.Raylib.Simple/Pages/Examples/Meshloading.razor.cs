using System.Numerics;
using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Examples;

public partial class Meshloading : IDisposable
{
    [Inject]
    public required ResourceService ResourceService { get; set; }

    private Camera3D _camera;
    private Model  _model;
    private async void Init()
    {
        const int screenWidth = 1280;
        const int screenHeight = 768;

        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
       
        RaylibExtensions.SetLoadFileTextCallback(ResourceService.GetLoadedResource);
        RaylibExtensions.SetLoadFileDataCallback(ResourceService.GetLoadedResource);

        await ResourceService.PreloadResource("resources/models/gltf/raylib_32x32.glb");
        
        _camera.Position = new Vector3 ( 10.0f, 10.0f, 10.0f ); 
        _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );      
        _camera.Up = new Vector3 ( 0.0f, 1.0f, 0.0f );         
        _camera.FovY = 45.0f;                                
        _camera.Projection = CameraProjection.Perspective;

        _model = LoadModel("resources/models/gltf/raylib_32x32.glb");
        OnResize((screenWidth, screenHeight));

    }
    
    // Main game loop
    private async void Render(float delta)
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);
        BeginDrawing();

            ClearBackground(Color.White);
            
            BeginMode3D(_camera);
                DrawModel(_model, Vector3.Zero, .2f, Color.White);
            EndMode3D();
                
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