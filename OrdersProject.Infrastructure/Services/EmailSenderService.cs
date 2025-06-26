using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using OrdersProject.Application.Common;
using OrdersProject.Domain.Interfaces;

namespace OrdersProject.Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSettings _emailSettings;

    public EmailSenderService(IOptions<EmailSettings> emailOptions)
    {
        _emailSettings = emailOptions.Value;
    }

    public async Task SendTestOrderEmailAsync(string? toAddress = null)
    {
        var to = toAddress ?? _emailSettings.Username;
        var orderNumber = Random.Shared.Next(1000, 9999);
        var orderDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Simple Light Candle", _emailSettings.Username));
        message.To.Add(new MailboxAddress("Simple Light Candle", to));
        message.Subject = $"[Simple Light Candle]: Nowe zamówienie {orderNumber}";

        var htmlBody = $@"<!DOCTYPE html><html lang='pl-PL'>
        <head><meta charset='UTF-8'></head><body>
        <h1>Nowe zamówienie: {orderNumber}</h1>
        <p>Otrzymałeś następujące zamówienie od <strong>Jan Kowalski</strong>:</p>
        <h2>
            <a href='https://example.com/wp-admin/admin.php?page=wc-orders&amp;action=edit&amp;id={orderNumber}'>
                [Zamówienie {orderNumber}]
            </a> ({orderDate})
        </h2>
        <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse;'>
            <thead><tr><th>Produkt</th><th>Ilość</th><th>Cena</th></tr></thead>
            <tbody>
                <tr><td>Dyfuzor zapachowy z patyczkami Chanel Woman</td><td>1</td><td>79,00 zł</td></tr>
                <tr><td>Dyfuzor zapachowy z patyczkami Pink Peony Garden</td><td>1</td><td>79,00 zł</td></tr>
                <tr><td>Wosk zapachowy Chanel Woman</td><td>1</td><td>29,00 zł</td></tr>
                <tr><td>Kominek zapachowy - Beczułka, Krzyżyki</td><td>1</td><td>59,00 zł</td></tr>
                <tr><td>Chanel Woman - Zapach do Samochodu</td><td>1</td><td>49,00 zł</td></tr>
            </tbody>
            <tfoot>
                <tr><th colspan='2'>Kwota:</th><td>295,00 zł</td></tr>
                <tr><th colspan='2'>Wysyłka:</th><td>Darmowa wysyłka</td></tr>
                <tr><th colspan='2'>Metoda płatności:</th><td>Płatność kartą z PayU</td></tr>
                <tr><th colspan='2'>Razem:</th><td>295,00 zł</td></tr>
            </tfoot>
        </table>
        <p>Adres dostawy: Jan Kowalski, Polna 719, 34-733 Polna</p>
        <p>Dziękujemy za zamówienie!</p>
        </body></html>";

        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, _emailSettings.SmtpUseSsl);
        await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
