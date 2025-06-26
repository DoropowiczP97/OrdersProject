using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrdersProject.Application.DTOs.Orders;
using OrdersProject.Application.Features.Orders.Commands;
using OrdersProject.Application.Interfaces;
using OrdersProject.Domain.Interfaces;
using System.Text;

namespace OrdersProject.Infrastructure.Services;

public class InboundEmailProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InboundEmailProcessingService> _logger;

    public InboundEmailProcessingService(
        IServiceProvider serviceProvider,
        ILogger<InboundEmailProcessingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InboundEmailProcessingService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                await ProcessEmailsAsync(scope.ServiceProvider, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd globalny podczas przetwarzania maili.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _logger.LogInformation("InboundEmailProcessingService stopped.");
    }
    private async Task ProcessEmailsAsync(IServiceProvider provider, CancellationToken ct)
    {
        var emailRepo = provider.GetRequiredService<IInboundEmailRepository>();
        var orderRepo = provider.GetRequiredService<IOrderRepository>();
        var parser = provider.GetRequiredService<IMailParserService>();
        var mediator = provider.GetRequiredService<IMediator>();
        var imapService = provider.GetRequiredService<ImapService>();

        await imapService.FetchUnreadEmailsAsync(ct);

        var emails = await emailRepo.GetUnparsedAsync();

        foreach (var email in emails)
        {
            var html = Encoding.UTF8.GetString(email.RawContent);

            ParsedOrderDto parsed;
            try
            {
                parsed = await parser.ParseEmailAsync(html);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Błąd parsowania maila {email.Id}, pominięty.");
                continue;
            }

            var orderCommand = new CreateOrderCommand
            {
                CustomerName = parsed.CustomerName,
                SourceEmail = parsed.SourceEmail,
                OrderDate = parsed.OrderDate,
                PaymentMethod = parsed.PaymentMethod,
                ShippingCost = parsed.ShippingCost,
                TotalAmount = parsed.TotalAmount,
                ShippingAddress = parsed.ShippingAddress,
                Items = parsed.Items.Select(i => new OrderItemDto
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            var returnedId = await mediator.Send(orderCommand);

            email.ParsedSuccessfully = true;
            email.OrderId = returnedId;

            await emailRepo.UpdateAsync(email);

            _logger.LogInformation($"Order {returnedId} utworzony z maila {email.Id}.");
        }
    }



}
