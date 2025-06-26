using OrdersProject.Application.DTOs.Orders;

namespace OrdersProject.Application.Interfaces;
public interface IMailParserService
{
    Task<List<ParsedOrderItem>> ParseEmailAsync(string emailHtml);

}
