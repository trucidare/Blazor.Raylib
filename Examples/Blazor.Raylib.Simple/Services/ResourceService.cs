namespace Blazor.Raylib.Simple.Services;

public sealed class ResourceService(HttpClient http)
{
    private Dictionary<string, byte[]> _loadedResources = [];
    
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
        => _loadedResources[filePath];
    
    public byte[] GetLoadedResource(string filePath, int _)
        => _loadedResources[filePath];
}