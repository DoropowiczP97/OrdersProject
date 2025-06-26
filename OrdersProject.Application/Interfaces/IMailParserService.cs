using OrdersProject.Application.DTOs.Orders;

namespace OrdersProject.Application.Interfaces;
public interface IMailParserService
{
    Task<ParsedOrderDto> ParseEmailAsync(string emailHtml);

}
