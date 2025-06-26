using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersProject.Application.Features.Orders.Commands;
using OrdersProject.Application.Features.Orders.Queries;
using OrdersProject.Application.Features.Orders.Queries.GetPageable;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.API.Endpoints;

public static class OrdersApi
{
    public static void MapOrders(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .WithOpenApi();

        #region Commands

        group.MapPost("/", async ([FromServices] ISender mediator, [FromBody] CreateOrderCommand command) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/orders/{id}", id);
        });
        group.MapPost("/send-test-email", async ([FromServices] IEmailSenderService emailSender) =>
        {
            await emailSender.SendTestOrderEmailAsync();
            return Results.Ok();
        });

        #endregion
        #region Queries

        group.MapGet("/", async ([FromServices] ISender mediator, [AsParameters] GetPageableOrdersQuery query) =>
        {
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async ([FromServices] ISender mediator, Guid id) =>
        {
            var result = await mediator.Send(new GetOrderByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        #endregion

    }
}