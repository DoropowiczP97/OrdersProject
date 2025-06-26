using OrdersProject.Domain.Entities;

namespace OrdersProject.Domain.Interfaces;

public interface IInboundEmailRepository
{
    Task AddAsync(InboundEmail email);
    Task<List<InboundEmail>> GetUnparsedAsync();
    Task UpdateAsync(InboundEmail email);
    Task MarkAsParsedAsync(Guid id);
    Task<bool> ExistsByExternalIdAsync(int externalId);

}