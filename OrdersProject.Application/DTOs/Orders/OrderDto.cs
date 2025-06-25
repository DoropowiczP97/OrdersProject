namespace OrdersProject.Application.DTOs.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string SourceEmail { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public byte[]? RawEmail { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
