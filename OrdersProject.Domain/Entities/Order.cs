namespace OrdersProject.Domain.Entities;
public class Order
{
    public Guid Id { get; set; }
    public string SourceEmail { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
