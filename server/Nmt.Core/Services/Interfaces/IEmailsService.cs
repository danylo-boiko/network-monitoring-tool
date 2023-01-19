using Nmt.Domain.Common;

namespace Nmt.Core.Services.Interfaces;

public interface IEmailsService : IService
{
    public Task SendEmailAsync(string receiverEmail, string subject, string text, CancellationToken cancellationToken);
}