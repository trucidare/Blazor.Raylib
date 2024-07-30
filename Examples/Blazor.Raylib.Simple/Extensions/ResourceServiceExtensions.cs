using Blazor.Raylib.Simple.Services;

namespace Blazor.Raylib.Simple.Extensions;

public static class ResourceServiceExtensions
{
    public static async Task<T> LoadResourceFromUri<T>(this ResourceService resourceService, string uri, Func<byte[], T> callback)
    {
        var resource = await resourceService.GetResource(uri);
        if (callback != null!)
            return callback(resource);
        
        return default!;
    }
}