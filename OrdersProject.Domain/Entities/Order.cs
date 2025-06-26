namespace OrdersProject.Domain.Entities;
public class Order
{
    public Guid Id { get; set; }
    public string SourceEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
}
