using MediatR;
using OrdersProject.Application.DTOs.Orders;

namespace OrdersProject.Application.Features.Orders.Commands;

public class CreateOrderCommand : IRequest<Guid>
{
    public string CustomerName { get; set; }
    public string SourceEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public string PaymentMethod { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public bool IsFromEmail { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}
