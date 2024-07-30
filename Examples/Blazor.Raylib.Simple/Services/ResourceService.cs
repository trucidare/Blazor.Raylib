namespace Blazor.Raylib.Simple.Services;

public sealed class ResourceService(HttpClient http)
{
    public async Task<byte[]> GetResource(string resourceName)
    {
        return await http.GetByteArrayAsync(resourceName);
    }
}