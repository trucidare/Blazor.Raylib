using System.Numerics;
using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Extensions;
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
    private Shader _shader;
    private RenderTexture2D _target;
    private Music _music;
    
    private async void Init()
    {
        const int screenWidth = 1280;
        const int screenHeight = 768;
        const int GLSL_VERSION = 100;
        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");
        InitAudioDevice();                  
        _color = Color.White;
        RaylibExtensions.SetLoadFileTextCallback(ResourceService.GetLoadedResource);
        RaylibExtensions.SetLoadFileDataCallback(ResourceService.GetLoadedResource);

        await ResourceService.PreloadResource("resources/models/obj/turret.obj");
        await ResourceService.PreloadResource("resources/models/obj/turret_diffuse.png");
        await ResourceService.PreloadResource($"resources/shaders/glsl{GLSL_VERSION}/grayscale.fs");
        await ResourceService.PreloadResource("resources/audio/mini1111.xm");
        
        _camera.Position = new Vector3 ( 10.0f, 10.0f, 10.0f ); 
        _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );      
        _camera.Up = new Vector3 ( 0.0f, 1.0f, 0.0f );         
        _camera.FovY = 45.0f;                                
        _camera.Projection = CameraProjection.Perspective;           
        
        _model = LoadModel("resources/models/obj/turret.obj");
        _texture = LoadTexture("resources/models/obj/turret_diffuse.png");

        unsafe { _model.Materials[0].Maps[(int)MaterialMapIndex.Diffuse].Texture = _texture; }
        
        _shader = LoadShader(null!, $"resources/shaders/glsl{GLSL_VERSION}/grayscale.fs");
        _target = LoadRenderTexture(screenWidth, screenHeight);

        _music = await ResourceService.LoadResourceFromUri("resources/audio/mini1111.xm", e => LoadMusicStreamFromMemory(".xm",e));
        PlayMusicStream(_music);
    }
    
    // Main game loop
    private async Task Render(float delta)
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);
        UpdateMusicStream(_music);
        
        BeginTextureMode(_target);
            ClearBackground(_color);
            
            BeginMode3D(_camera);
                DrawModel(_model, Vector3.Zero, .3f, Color.White);   
                DrawGrid(10, 1.0f);          
            EndMode3D();
        EndTextureMode();

        BeginDrawing();
            ClearBackground(Color.White);
            BeginShaderMode(_shader);
                DrawTextureRec(_target.Texture,
                    new Rectangle(0, 0, _target.Texture.Width, -_target.Texture.Height),
                    new Vector2(0, 0),
                    Color.White);
            EndShaderMode();
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