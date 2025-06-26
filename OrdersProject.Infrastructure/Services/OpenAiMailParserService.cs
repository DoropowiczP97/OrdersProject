using Microsoft.Extensions.Options;
using OrdersProject.Application.Common;
using OrdersProject.Application.DTOs.Orders;
using OrdersProject.Application.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OrdersProject.Infrastructure.Services;

public class OpenAiMailParserService : IMailParserService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiMailParserService(HttpClient httpClient, IOptions<OpenAiSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task<List<ParsedOrderItem>> ParseEmailAsync(string emailHtml)
    {
        var prompt = $"Zwróć JSON z listą produktów (ProductName, Quantity, Price) z tego HTML maila: {emailHtml}";

        var requestBody = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = 0.2
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var content = JsonDocument.Parse(json)
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            return new List<ParsedOrderItem>();

        var parsed = JsonSerializer.Deserialize<List<ParsedOrderItem>>(content!);
        return parsed ?? new();
    }
}
