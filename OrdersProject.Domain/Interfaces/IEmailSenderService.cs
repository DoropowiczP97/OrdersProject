namespace OrdersProject.Domain.Interfaces;

public interface IEmailSenderService
{
    Task SendTestOrderEmailAsync(string? toAddress = null);
}
