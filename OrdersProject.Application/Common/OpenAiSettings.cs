namespace OrdersProject.Application.Common;

public class OpenAiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public float Temperature { get; set; } = 0.7f;
}
