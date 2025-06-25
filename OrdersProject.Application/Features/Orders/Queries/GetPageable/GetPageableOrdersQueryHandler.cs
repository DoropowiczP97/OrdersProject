using MediatR;
using OrdersProject.Application.Common;
using OrdersProject.Application.DTOs.Orders;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.Application.Features.Orders.Queries.GetPageable;

public class GetPageableOrdersQueryHandler : IRequestHandler<GetPageableOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetPageableOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<OrderDto>> Handle(GetPageableOrdersQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _orderRepository.CountAsync();

        var orders = await _orderRepository.GetPagedAsync(request.PageNumber, request.PageSize);

        var dtoList = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            SourceEmail = o.SourceEmail,
            OrderDate = o.OrderDate,
            PaymentMethod = o.PaymentMethod,
            ShippingCost = o.ShippingCost,
            TotalAmount = o.TotalAmount,
            ShippingAddress = o.ShippingAddress,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        }).ToList();

        return new PagedResult<OrderDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
