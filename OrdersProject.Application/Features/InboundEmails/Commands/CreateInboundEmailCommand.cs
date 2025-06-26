using MediatR;

namespace OrdersProject.Application.Features.InboundEmails.Commands;
public record AddInboundEmailCommand(
    string? From,
    string? Subject,
    DateTime ReceivedAt,
    byte[] RawContent
) : IRequest<Guid>;