using MediatR;
using OrdersProject.Application.DTOs.Orders;

namespace OrdersProject.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;