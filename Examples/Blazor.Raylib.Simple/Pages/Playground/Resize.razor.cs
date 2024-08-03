using System.Numerics;
using Blazor.Raylib.Extensions;
using Blazor.Raylib.Simple.Services;
using Microsoft.AspNetCore.Components;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Simple.Pages.Playground;

public partial class Resize : ComponentBase
{
    [Inject]
    public required ResourceService ResourceService { get; set; }
    
    private Camera3D _camera;
    private Model _model;
    private unsafe ModelAnimation* _modelAnim;
    private int _animCurrentFrame = 0;
    private int _animIndex = 0;
    private async void Init()
    {
        InitWindow(1280,768, "Resize sample");       
        
        RaylibExtensions.SetLoadFileTextCallback(ResourceService.GetLoadedResource);
        RaylibExtensions.SetLoadFileDataCallback(ResourceService.GetLoadedResource);

        await ResourceService.PreloadResource("resources/models/gltf/robot.glb");
        
        _camera.Position = new Vector3(10.0f,10.0f,10.0f);
        _camera.Target = new Vector3(0f, 0f, 0f);
        _camera.Up = new Vector3(0f, 1f, 0f);
        _camera.FovY = 45f;
        _camera.Projection = CameraProjection.Perspective;

        _model = LoadModel("resources/models/gltf/robot.glb");
        
        int nums = 0;
        unsafe
        {
            var m = LoadModelAnimations("resources/models/gltf/robot.glb", ref nums);
            _modelAnim = m;
        }
    }

    private void Render(float deltaTime)
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);

        unsafe
        {
            if (_modelAnim != null)
            {
                ModelAnimation anim = _modelAnim[_animIndex];
                _animCurrentFrame = (_animCurrentFrame + 1) % anim.FrameCount;
                UpdateModelAnimation(_model, anim, _animCurrentFrame);
            }
        }

        BeginDrawing();
            ClearBackground(ColorFromHSV(0,0,0));
            DrawFPS(10,10);
            
            BeginMode3D(_camera);
                DrawGrid(100,1);
                DrawModel(_model, Vector3.Zero, 0.5f, Color.White);
            EndMode3D();
        EndDrawing();
    }

    private void OnResize((int width, int height) Size)
    {
        SetWindowSize(Size.width, Size.height);
    }
}

// https://github.com/raysan5/raylib/blob/master/examples/models/models_mesh_picking.c
// https://github.com/raysan5/physac
// https://github.com/raysan5/raylib/blob/master/examples/models/rlights.h
// https://github.com/raysan5/raygui