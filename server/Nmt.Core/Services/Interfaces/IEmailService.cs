using Nmt.Domain.Common;

namespace Nmt.Core.Services.Interfaces;

public interface IEmailService : IService
{
    public Task SendMessageAsync(string receiverEmail, string subject, string text, CancellationToken cancellationToken);
}