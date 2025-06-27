using Microsoft.EntityFrameworkCore;
using OrdersProject.Domain.Entities;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.Infrastructure.Persistence.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string? sortDirection = "asc")
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        IQueryable<Order> query = _context.Orders
            .AsNoTracking()
            .Include(o => o.Items);

        if (!string.IsNullOrEmpty(sortBy))
        {
            var isDescending = sortDirection?.ToLower() == "desc";

            query = isDescending
                ? query.OrderByDescending(o => EF.Property<object>(o, sortBy))
                : query.OrderBy(o => EF.Property<object>(o, sortBy));
        }
        else
        {
            query = query.OrderByDescending(o => o.OrderDate);
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }


    public async Task<int> CountAsync()
    {
        return await _context.Orders.CountAsync();
    }
}
