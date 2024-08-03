using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.Json;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Blazor.Raylib.Extensions;

public delegate byte[] LoadTextFileCallback(string resource);
public delegate void RenderCallback(float delta);

public struct Light
{
    public LightType Type;
    public bool Enabled;
    public Vector3 Position;
    public Vector3 Target;
    public Color Color;
    public float Attenuation;

    public int EnabledLoc;
    public int TypeLoc;
    public int PositionLoc;
    public int TargetLoc;
    public int ColorLoc;
    public int AttenuationLoc;
}

public enum LightType : uint
{
    LightDirectional = 0,
    LightPoint
}

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class RaylibExtensions
{
    
    private const string NativeLibName = "raylib";

    [DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
    private static extern Image LoadImageAnimFromMemory(sbyte* fileName, byte* fileData, int dataSize, int* frames);
    
    [DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Ray GetScreenToWorldRay(Vector2 mousePosition, Camera3D camera);
    
    
    [DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "emscripten_set_main_loop")]
    private static extern void SetMainLoop(delegate* unmanaged[Cdecl]<float, void> callback, int fps, int infiniteLoop);
    
    #region Lights

    public const int MaxLights = 4;
    
    private static int _lightsCount = 0; 
    
    public static Light CreateLight(LightType type, Vector3 position, Vector3 target, Color color, Shader shader)
    {
        Light light = new();

        if (_lightsCount < MaxLights)
        {
            light.Enabled = true;
            light.Type = type;
            light.Position = position;
            light.Target = target;
            light.Color = color;

            // NOTE: Lighting shader naming must be the provided ones
            light.EnabledLoc =  GetShaderLocation(shader, $"lights[{_lightsCount}].enabled");
            light.TypeLoc = GetShaderLocation(shader, $"lights[{_lightsCount}].type");
            light.PositionLoc = GetShaderLocation(shader, $"lights[{_lightsCount}].position");
            light.TargetLoc = GetShaderLocation(shader, $"lights[{_lightsCount}].target");
            light.ColorLoc = GetShaderLocation(shader, $"lights[{_lightsCount}].color");
            
            UpdateLightValues(shader, light);
        
            _lightsCount++;
        }

        return light;
    }

    public static void UpdateLightValues(Shader shader, Light light)
    {
        // Send to shader light enabled state and type
        SetShaderValue(shader, light.EnabledLoc, light.Enabled, ShaderUniformDataType.Int);
        SetShaderValue(shader, light.TypeLoc, light.Type, ShaderUniformDataType.Int);

        // Send to shader light position values
        float[] position = [ light.Position.X, light.Position.Y, light.Position.Z ];
        SetShaderValue(shader, light.PositionLoc, position, ShaderUniformDataType.Vec3);

        // Send to shader light target position values
        float[] target = [ light.Target.X, light.Target.Y, light.Target.Z ];
        SetShaderValue(shader, light.TargetLoc, target, ShaderUniformDataType.Vec3);

        // Send to shader light color values
        float[] color = [ light.Color.R/255f, light.Color.G/255f, light.Color.B/255f, light.Color.A/255f ];
        SetShaderValue(shader, light.ColorLoc, color, ShaderUniformDataType.Vec4);
    }

    
    #endregion


    public static void SetLoadFileDataCallback(LoadTextFileCallback callback)
    {
        if (LoadFileBag.Callback == null!)
        {
            LoadFileBag.Callback = callback;
            Raylib_cs.Raylib.SetLoadFileDataCallback(&LoadFileBag.Processor);
        }
    }
    
    public static void SetLoadFileTextCallback(LoadTextFileCallback callback)
    {
        if (LoadFileBag.TextFileCallback == null!)
        {
            LoadFileBag.TextFileCallback = callback;
            Raylib_cs.Raylib.SetLoadFileTextCallback(&LoadFileBag.TextProcessor);
        }
    }
    
    
    public static string TextSubtext(string text,  int position, int length)
    {
        using var str1 = text.ToUtf8Buffer();
        var result = Raylib_cs.Raylib.TextSubtext(str1.AsPointer(), position, length);
        var res = Utf8StringUtils.GetUTF8String(result);
        Raylib_cs.Raylib.MemFree(result);
        return res;
    }
    
    public static Image LoadImageAnimFromMemory(string fileName, byte[] fileData, out int frameCount)
    {
        int frames;
        using var fileNameNative = fileName.ToAnsiBuffer();
        fixed (byte* fileDataNative = fileData)
        {
            Image image = LoadImageAnimFromMemory(fileNameNative.AsPointer(), fileDataNative, fileData.Length, &frames);
            frameCount = frames;
            return image;
        }
    }

    public static void SetMainLoop(RenderCallback callback, int fps)
    {
        MainLoopHelper.Callback = callback;
        SetMainLoop(&MainLoopHelper.Renders, 0,0);
    }
}

internal static unsafe class LoadFileBag
{
    public static LoadTextFileCallback Callback = null!;
    
    public static LoadTextFileCallback TextFileCallback = null!;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static byte* Processor(sbyte* buffer, int* length)
    {
        var resource = Utf8StringUtils.GetUTF8String(buffer);
        if (!string.IsNullOrEmpty(resource))
        {
            Console.WriteLine($"DEBUG: {resource}");
            var bytes = Callback?.Invoke(resource) ?? [];
            fixed (byte* p = &bytes[0] )
            {
                *length = bytes.Length;
                return p;
            }
        }

        return null;
    }
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static sbyte* TextProcessor(sbyte* buffer)
    {
        var resource = Utf8StringUtils.GetUTF8String(buffer);
        if (!string.IsNullOrEmpty(resource))
        {
            Console.WriteLine($"DEBUG: {resource}");
            var bytes = TextFileCallback?.Invoke(resource) ?? [];
            var s = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length).ToUtf8Buffer();
            return s.AsPointer();
        }

        return null;
    }
}

internal static unsafe class MainLoopHelper
{

    public static RenderCallback Callback = null!;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static async void Renders(float delta)
    {
        Callback?.Invoke(delta);
    }
}