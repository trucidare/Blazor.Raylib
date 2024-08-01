namespace Blazor.Raylib.Simple.Services;

public sealed class ResourceService(HttpClient http)
{
    private readonly Dictionary<string, byte[]> _loadedResources = [];
    
    public async Task<byte[]> GetResource(string resourceName)
    {
        return await http.GetByteArrayAsync(resourceName);
    }

    public async Task PreloadResource(string resourceName)
    {
        var resource = await GetResource(resourceName);
        _loadedResources[resourceName] = resource;
    }

    public byte[] GetLoadedResource(string filePath)
        => CheckAndGetResource(filePath);
    
    public byte[] GetLoadedResource(string filePath, int _)
        => CheckAndGetResource(filePath);

    private byte[] CheckAndGetResource(string resourceName)
    {
        if (_loadedResources.TryGetValue(resourceName, out var resource))
            return resource;

        return [];
    }
}