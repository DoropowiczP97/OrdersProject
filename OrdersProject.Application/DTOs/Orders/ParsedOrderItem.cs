namespace OrdersProject.Application.DTOs.Orders;

public class ParsedOrderItem
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}