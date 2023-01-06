using MediatR;

namespace Nmt.Core.Cache.Interfaces;

public interface ICachePolicy<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromHours(1);
    string GetCacheKey(TRequest request);
}