using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersProject.Application.Features.Orders.Commands;
using OrdersProject.Application.Features.Orders.Queries.GetPageable;

namespace OrdersProject.API.Endpoints;

public static class OrdersApi
{
    public static void MapOrders(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .WithOpenApi();

        group.MapGet("/", async ([FromServices] ISender mediator, [AsParameters] GetPageableOrdersQuery query) =>
        {
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapPost("/", async ([FromServices] ISender mediator, [FromBody] CreateOrderCommand command) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/orders/{id}", id);
        });

    }
}