namespace OrdersProject.Domain.Entities;
public class InboundEmail
{
    public Guid Id { get; set; }
    public string? From { get; set; }
    public string? Subject { get; set; }
    public DateTime ReceivedAt { get; set; }
    public byte[] RawContent { get; set; } = Array.Empty<byte>();
    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }
    public bool ParsedSuccessfully { get; set; } = false;
    public int? ExternalId { get; set; }
}
