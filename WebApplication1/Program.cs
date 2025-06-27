using Consul;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Load API key from config or environment variable
string openAiApiKey = builder.Configuration["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;

// Register HttpClient
builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiApiKey);
});

// Register Consul client
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri("http://localhost:8500"); // Adjust for your Consul server
}));

var app = builder.Build();

// Register with Consul
var consulClient = app.Services.GetRequiredService<IConsulClient>();

var serviceId = $"chatgpt-service-{Guid.NewGuid()}";
var registration = new AgentServiceRegistration()
{
    ID = serviceId,
    Name = "chatgpt-service",
    Address = "localhost", // Replace with your container/host IP if needed
    Port = app.Urls
        .Select(url => new Uri(url).Port)
        .FirstOrDefault() // Gets the running port
};

await consulClient.Agent.ServiceRegister(registration);

app.Lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(serviceId).Wait();
});

// Chat endpoint
app.MapPost("/chat", async (HttpClient httpClient, string prompt) =>
{
    var requestBody = new
    {
        model = "gpt-4",
        messages = new[]
        {
            new { role = "user", content = prompt }
        }
    };

    var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
    var response = await httpClient.PostAsync("chat/completions", content);
    response.EnsureSuccessStatusCode();

    var json = await response.Content.ReadAsStringAsync();
    var result = JsonDocument.Parse(json);

    return Results.Ok(result.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString());
});

app.Run();
