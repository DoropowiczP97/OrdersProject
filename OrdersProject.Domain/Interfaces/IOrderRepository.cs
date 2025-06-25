using OrdersProject.Domain.Entities;

namespace OrdersProject.Domain.Interfaces;
public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<List<Order>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> CountAsync();
}
