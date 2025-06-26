using Microsoft.EntityFrameworkCore;
using OrdersProject.Domain.Entities;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.Infrastructure.Persistence.Repositories;

public class InboundEmailRepository : IInboundEmailRepository
{
    private readonly OrderDbContext _context;

    public InboundEmailRepository(OrderDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(InboundEmail email)
    {
        ArgumentNullException.ThrowIfNull(email);

        await _context.InboundEmails.AddAsync(email);
        await _context.SaveChangesAsync();
    }

    public async Task<List<InboundEmail>> GetUnparsedAsync()
    {
        return await _context.InboundEmails
            .Where(e => !e.ParsedSuccessfully)
            .AsNoTracking()
            .OrderBy(e => e.ReceivedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(InboundEmail email)
    {
        ArgumentNullException.ThrowIfNull(email);

        _context.InboundEmails.Update(email);
        await _context.SaveChangesAsync();
    }
}
