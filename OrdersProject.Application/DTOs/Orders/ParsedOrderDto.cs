namespace OrdersProject.Application.DTOs.Orders;

public class ParsedOrderDto
{
    public string CustomerName { get; set; } = default!;
    public string SourceEmail { get; set; } = default!;
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; } = default!;
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = default!;
    public List<ParsedOrderItemDto> Items { get; set; } = new();
}