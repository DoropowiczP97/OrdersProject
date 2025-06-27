using Microsoft.Extensions.Options;
using OrdersProject.Application.Common;
using OrdersProject.Application.DTOs.Orders;
using OrdersProject.Application.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OrdersProject.Infrastructure.Services;

public class OpenAiMailParserService : IMailParserService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiSettings _settings;

    public OpenAiMailParserService(HttpClient httpClient, IOptions<OpenAiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<ParsedOrderDto> ParseEmailAsync(string emailHtml)
    {
        var prompt = $$"""
            Zamień poniższy HTML maila WooCommerce na JSON zamówienia w formacie:
            {
              "CustomerName": string,
              "SourceEmail": string,
              "OrderDate": "yyyy-MM-dd",
              "PaymentMethod": string,
              "ShippingCost": decimal,
              "TotalAmount": decimal,
              "ShippingAddress": string,
              "Items": [
                {
                  "ProductName": string,
                  "Quantity": int,
                  "Price": decimal
                }
              ]
            }

            Zwróć tylko JSON. Email HTML:
            {{emailHtml}}
        """;

        var requestBody = new
        {
            model = _settings.Model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = _settings.Temperature
        };

        var request = new HttpRequestMessage(HttpMethod.Post, _settings.Endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Limit zapytań przekroczony (OpenAI): {body}", inner: null, statusCode: response.StatusCode);
        }

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var root = JsonDocument.Parse(json).RootElement;

        var content = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            throw new HttpRequestException("Odpowiedź modelu była pusta.",inner: null,statusCode: HttpStatusCode.NoContent);

        var parsed = JsonSerializer.Deserialize<ParsedOrderDto>(content!, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (parsed == null)
            throw new HttpRequestException("Nie udało się sparsować odpowiedzi modelu.",inner: null,statusCode: HttpStatusCode.UnprocessableEntity);

        return parsed;
    }
}