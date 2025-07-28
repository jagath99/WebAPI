namespace WebAPI.Services;
using System.Net.Http.Json;
using WebAPI.Models;

public class ThirdPartyAccess
{
    private readonly HttpClient _httpClient;
    public ThirdPartyAccess(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Post>($"http://jsonplaceholder.typicode.com/posts/{id}");
    }
    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Comment>($"http://jsonplaceholder.typicode.com/comments/{id}");
    }
}
