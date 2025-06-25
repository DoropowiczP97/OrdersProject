using MediatR;
using OrdersProject.Application.Common;
using OrdersProject.Application.DTOs.Orders;

namespace OrdersProject.Application.Features.Orders.Queries.GetPageable;

public class GetPageableOrdersQuery : IRequest<PagedResult<OrderDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

