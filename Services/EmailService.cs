using System.Net.Mail;
using System.Net;

namespace OfficeRoomie.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("Smtp");
        var host = smtpSettings["Host"];
        var port = int.Parse(smtpSettings["Port"]!);
        var enableSsl = bool.Parse(smtpSettings["EnableSsl"]!);
        var user = smtpSettings["User"];
        var password = smtpSettings["Password"];

        using var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(user, password),
            EnableSsl = enableSsl,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(user!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }

}
