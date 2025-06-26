namespace OrdersProject.Application.Common;

public class EmailSettings
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // IMAP 
    public string ImapHost { get; set; } = "imap.gmail.com";
    public int ImapPort { get; set; } = 993;
    public bool ImapUseSsl { get; set; } = true;

    // SMTP
    public string SmtpHost { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public bool SmtpUseSsl { get; set; } = false; 
}
