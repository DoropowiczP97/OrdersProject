namespace OrdersProject.Application.Common;

public class OpenAiSettings
{
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }
    public string Model { get; set; }
    public float Temperature { get; set; } = 0.7f;
}
