using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatGptService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<string> GetChatGptResponse(string prompt)
    {
        var request = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json);
        return data.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }
}
