using MediatR;

namespace Nmt.Domain.Common;

public interface ICachePolicy<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromHours(1);
    public string GetCacheKey(TRequest request);
}