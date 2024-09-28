using System.Numerics;
using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Playground;

public partial class Interaction : IDisposable
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
    private Shader _lightShader;
    private RenderTexture2D _target;
    private Music _music;
    private Light[] _lights = [];
    private bool _initialized;
    
    private async void Init()
    {
        const int screenWidth = 1280;
        const int screenHeight = 768;
        
        SetConfigFlags(ConfigFlags.Msaa4xHint);
        InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window"); 
        InitAudioDevice();      
       
        _color = Color.Black;
        RaylibExtensions.SetLoadFileTextCallback(ResourceService.GetLoadedResource);
        RaylibExtensions.SetLoadFileDataCallback(ResourceService.GetLoadedResource);

        await ResourceService.PreloadResource("resources/models/objs/turret.obj");
        await ResourceService.PreloadResource("resources/models/objs/turret_diffuse.png");
        await ResourceService.PreloadResource($"resources/shaders/glsl100/sobel.fs");
        await ResourceService.PreloadResource("resources/audio/mini1111.xm");
        await ResourceService.PreloadResource("resources/shaders/glsl100/lighting.fs");
        await ResourceService.PreloadResource("resources/shaders/glsl100/lighting.vs");
        
        _camera.Position = new Vector3 ( 10.0f, 10.0f, 10.0f ); 
        _camera.Target = new Vector3( 0.0f, 2.0f, 0.0f );      
        _camera.Up = new Vector3 ( 0.0f, 1.0f, 0.0f );         
        _camera.FovY = 45.0f;                                
        _camera.Projection = CameraProjection.Perspective;           
        
        _model = LoadModel("resources/models/objs/turret.obj");
        _texture = LoadTexture("resources/models/objs/turret_diffuse.png");

        unsafe { _model.Materials[0].Maps[(int)MaterialMapIndex.Diffuse].Texture = _texture; }
        
        _shader = LoadShader(null!, $"resources/shaders/glsl100/sobel.fs");
        _target = LoadRenderTexture(screenWidth, screenHeight);
        
        _lightShader = LoadShader("resources/shaders/glsl100/lighting.vs", "resources/shaders/glsl100/lighting.fs");
        unsafe
        {
            _lightShader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_lightShader, "viewPos");
        }
        
        int ambientLoc = GetShaderLocation(_lightShader, "ambient");
        SetShaderValue(_lightShader, ambientLoc, [0.1f, 0.1f, 0.1f, 1.0f ], ShaderUniformDataType.Vec4);
        unsafe {_model.Materials[*_model.MeshMaterial].Shader = _lightShader;}

        // Create lights // FIXME: Missing raylight...
        _lights = new Light[RaylibExtensions.MaxLights];
        _lights[0] = RaylibExtensions.CreateLight(LightType.LightPoint, new Vector3( -2, 1, -2 ), Vector3.Zero, Color.Yellow, _lightShader);
        _lights[1] = RaylibExtensions.CreateLight(LightType.LightPoint, new Vector3( 2, 1, 2 ), Vector3.Zero, Color.Red, _lightShader);
        _lights[2] = RaylibExtensions.CreateLight(LightType.LightPoint, new Vector3( -2, 1, 2 ), Vector3.Zero, Color.Green, _lightShader);
        _lights[3] = RaylibExtensions.CreateLight(LightType.LightPoint, new Vector3( 2, 1, -2 ), Vector3.Zero, Color.Blue, _lightShader);
        
        _music = await ResourceService.LoadResourceFromUri("resources/audio/mini1111.xm", e => LoadMusicStreamFromMemory(".xm",e));
        PlayMusicStream(_music);

        foreach (var light in _lights)
            RaylibExtensions.UpdateLightValues(_lightShader, light);

        _initialized = true;
        OnResize((screenWidth, screenHeight));
    }
    
    // Main game loop
    private async void Render(float delta)
    {
        if (!_initialized)
            return;

        UpdateCamera(ref _camera, CameraMode.Orbital);
        Vector3 cameraPos = new(_camera.Position.X, _camera.Position.Y, _camera.Position.Z);

        unsafe
        {
            SetShaderValue(_lightShader, _lightShader.Locs[(int)ShaderLocationIndex.VectorView], cameraPos,
                ShaderUniformDataType.Vec3);
        }

        if (IsKeyPressed(KeyboardKey.Y))
        {
            _lights[0].Enabled = !_lights[0].Enabled;
        }

        if (IsKeyPressed(KeyboardKey.R))
        {
            _lights[1].Enabled = !_lights[1].Enabled;
        }

        if (IsKeyPressed(KeyboardKey.G))
        {
            _lights[2].Enabled = !_lights[2].Enabled;
        }

        if (IsKeyPressed(KeyboardKey.B))
        {
            _lights[3].Enabled = !_lights[3].Enabled;
        }


        foreach (var light in _lights)
            RaylibExtensions.UpdateLightValues(_lightShader, light);

        UpdateMusicStream(_music);

       BeginTextureMode(_target);
            ClearBackground(_color);

            BeginMode3D(_camera);
                DrawPlane(Vector3.Zero, new Vector2( 10.0f, 10.0f ), Color.White);
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

        StateHasChanged();
        await Task.CompletedTask;
    }

    private void ChangeColor()
    {
        _color = ColorFromHSV(Random.Shared.Next(), 1, 1);
    }
    
    private void OnResize((int width, int height) Size)
    {
        SetWindowSize(Size.width, Size.height);
        _target = LoadRenderTexture(Size.width, Size.height);

    }
    
    public void Dispose()
    {
        CloseWindow();
    }
}