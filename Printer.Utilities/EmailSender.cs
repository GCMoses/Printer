using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using DigiPixWebUtilities;
using MimeKit;

namespace Printer.Utilities;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse("devsoluz@gmail.com"));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

        var emailConfiguration = _config.GetSection("EmailConfiguration").Get<EmailConfiguration>();

        using (var emailClient = new SmtpClient())
        {
            emailClient.Connect(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }

        return Task.CompletedTask;
    }
}
