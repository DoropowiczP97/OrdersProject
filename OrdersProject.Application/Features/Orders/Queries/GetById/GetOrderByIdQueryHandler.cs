using MediatR;
using OrdersProject.Application.DTOs.Orders;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.Application.Features.Orders.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id);
        if (order is null) return null;

        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            SourceEmail = order.SourceEmail,
            OrderDate = order.OrderDate,
            PaymentMethod = order.PaymentMethod,
            ShippingCost = order.ShippingCost,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }
}
