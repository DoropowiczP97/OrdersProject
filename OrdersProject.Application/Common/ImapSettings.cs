namespace OrdersProject.Application.Common;

public class ImapSettings
{
    public string Host { get; set; } = "imap-mail.outlook.com"; 
    public int Port { get; set; } = 993;
    public bool UseSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
