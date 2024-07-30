using System.Runtime.InteropServices;
using System.Security;
using Raylib_cs;

namespace Blazor.Raylib.Extensions;

[SuppressUnmanagedCodeSecurity]
public static unsafe partial class RaylibExtensions
{
    public const string NativeLibName = "raylib";

    [DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Image LoadImageAnimFromMemory(sbyte* fileName, byte* fileData, int dataSize, int* frames);
    
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
}