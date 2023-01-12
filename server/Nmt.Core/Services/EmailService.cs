using System.Net;
using System.Net.Mail;
using Nmt.Core.Services.Interfaces;
using Nmt.Domain.Configs;

namespace Nmt.Core.Services;

public class EmailService : IEmailService
{
    private readonly GoogleSmtpConfig _googleSmtpConfig;
    private readonly MailAddress _fromAddress;

    public EmailService(GoogleSmtpConfig googleSmtpConfig)
    {
        _googleSmtpConfig = googleSmtpConfig;
        _fromAddress = new MailAddress(googleSmtpConfig.Email, googleSmtpConfig.Name);
    }

    public async Task SendEmailAsync(string receiverEmail, string subject, string text, CancellationToken cancellationToken)
    {
        var emailMessage = new MailMessage(_fromAddress, new MailAddress(receiverEmail))
        {
            Subject = subject,
            Body = text
        };

        using var smtpClient = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_googleSmtpConfig.Email, _googleSmtpConfig.Password)
        };

        await smtpClient.SendMailAsync(emailMessage, cancellationToken);
    }
}