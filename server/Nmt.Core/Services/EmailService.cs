using Nmt.Core.Services.Interfaces;

namespace Nmt.Core.Services;

public class EmailService : IEmailService
{
    public async Task SendMessageAsync(string receiverEmail, string subject, string text, CancellationToken cancellationToken)
    {
        Console.WriteLine(text);
    }
}