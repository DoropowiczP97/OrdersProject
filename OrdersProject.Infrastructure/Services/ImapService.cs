using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using OrdersProject.Application.Common;
using OrdersProject.Domain.Entities;
using OrdersProject.Domain.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace OrdersProject.Infrastructure.Services;

public class ImapService
{
    private readonly EmailSettings _settings;
    private readonly IInboundEmailRepository _inboundEmailRepository;

    public ImapService(IOptions<EmailSettings> settings, IInboundEmailRepository inboundEmailRepository)
    {
        _settings = settings.Value;
        _inboundEmailRepository = inboundEmailRepository;
    }

    public async Task FetchUnreadEmailsAsync(CancellationToken cancellationToken)
    {
        using var client = new ImapClient();

        await client.ConnectAsync(_settings.ImapHost, _settings.ImapPort, _settings.ImapUseSsl, cancellationToken);

        client.AuthenticationMechanisms.Clear();

        client.AuthenticationMechanisms.Add("PLAIN");

        await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);

        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadWrite, cancellationToken);

        var uids = await inbox.SearchAsync(SearchQuery.NotSeen, cancellationToken);

        foreach (var uid in uids)
        {
            var message = await inbox.GetMessageAsync(uid, cancellationToken);

            int? externalId = null;

            var match = Regex.Match(message.Subject, @"Nowe zamówienie (\d+)");

            if (match.Success && int.TryParse(match.Groups[1].Value, out var parsedId))
            {
                externalId = parsedId;
            }

            if (externalId.HasValue && await _inboundEmailRepository.ExistsByExternalIdAsync(externalId.Value))
                continue;

            var email = new InboundEmail
            {
                Id = Guid.NewGuid(),
                From = message.From.Mailboxes.FirstOrDefault()?.Address ?? "",
                Subject = message.Subject ?? "",
                ReceivedAt = message.Date.DateTime,
                RawContent = Encoding.UTF8.GetBytes(message.ToString()),
                ExternalId = externalId
            };


            await _inboundEmailRepository.AddAsync(email);
        }

        await client.DisconnectAsync(true, cancellationToken);
    }

}