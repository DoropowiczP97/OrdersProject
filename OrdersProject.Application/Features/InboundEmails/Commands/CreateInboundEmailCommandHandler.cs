using MediatR;
using OrdersProject.Domain.Interfaces;
using OrdersProject.Domain.Entities;

namespace OrdersProject.Application.Features.InboundEmails.Commands;

public class AddInboundEmailCommandHandler : IRequestHandler<AddInboundEmailCommand, Guid>
{
    private readonly IInboundEmailRepository _repository;

    public AddInboundEmailCommandHandler(IInboundEmailRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddInboundEmailCommand request, CancellationToken cancellationToken)
    {
        var entity = new InboundEmail
        {
            Id = Guid.NewGuid(),
            From = request.From,
            Subject = request.Subject,
            ReceivedAt = request.ReceivedAt,
            RawContent = request.RawContent
        };

        await _repository.AddAsync(entity);
        return entity.Id;
    }
}