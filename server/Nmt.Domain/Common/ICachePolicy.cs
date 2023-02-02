using MediatR;

namespace Nmt.Domain.Common;

public interface ICachePolicy<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromHours(1);
    string GetCacheKey(TRequest request);
}