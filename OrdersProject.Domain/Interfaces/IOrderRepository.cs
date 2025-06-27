using OrdersProject.Domain.Entities;

namespace OrdersProject.Domain.Interfaces;
public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<List<Order>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc");
    Task<int> CountAsync();
    Task<Order?> GetByIdAsync(Guid id);
}
