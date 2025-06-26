namespace OrdersProject.Application.DTOs.Orders;

public class ParsedOrderItemDto
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}