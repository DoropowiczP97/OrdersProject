using MediatR;
using OrdersProject.Domain.Entities;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.Application.Features.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = request.CustomerName,
            SourceEmail = request.SourceEmail,
            OrderDate = request.OrderDate,
            PaymentMethod = request.PaymentMethod,
            ShippingCost = request.ShippingCost,
            TotalAmount = request.TotalAmount,
            ShippingAddress = request.ShippingAddress,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        await _orderRepository.AddAsync(order);
        return order.Id;
    }
}
