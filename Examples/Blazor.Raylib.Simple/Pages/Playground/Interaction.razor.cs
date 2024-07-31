using System.Numerics;
using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Playground;

public partial class Interaction : ComponentBase
{
    [Inject]
    public required ResourceService ResourceService { get; set; }
    
    private Color _color;
    private Vector2 _position;
    private int _fps;
    private Model _model;
    private Texture2D _texture;
    private Camera3D _camera;
    
    private async void Init()
    {
        const int screenWidth = 1280;
        const int screenHeight = 768;
        
        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
        _color = Color.White;
        RaylibExtensions.SetLoadFileTextCallback(ResourceService.GetLoadedResource);
        RaylibExtensions.SetLoadFileDataCallback(ResourceService.GetLoadedResource);

        await ResourceService.PreloadResource("resources/models/obj/turret.obj");
        await ResourceService.PreloadResource("resources/models/obj/turret_diffuse.png");

        _camera.Position = new Vector3 ( 10.0f, 10.0f, 10.0f ); // Camera position
        _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );      // Camera looking at point
        _camera.Up = new Vector3 ( 0.0f, 1.0f, 0.0f );          // Camera up vector (rotation towards target)
        _camera.FovY = 45.0f;                                // Camera field-of-view Y
        _camera.Projection = CameraProjection.Perspective;             // Camera projection type
        
        _model = LoadModel("resources/models/obj/turret.obj");
        _texture = LoadTexture("resources/models/obj/turret_diffuse.png");

        unsafe
        {
            _model.Materials[0].Maps[(int)MaterialMapIndex.Diffuse].Texture = _texture;
        }
    }
    
    // Main game loop
    private async Task Render(float delta)
    {
        BeginDrawing();
            ClearBackground(_color);
            
            BeginMode3D(_camera);
                DrawModel(_model, Vector3.Zero, .3f, Color.White);            
            EndMode3D();
        EndDrawing();
        
        _position = GetMousePosition();
        _fps = GetFPS();
        await Task.CompletedTask;
    }

    private void ChangeColor()
    {
        _color = ColorFromHSV(Random.Shared.Next(), 1, 1);
    }
}