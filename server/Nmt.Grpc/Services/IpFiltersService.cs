using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;
using Nmt.Domain.Consts;
using Nmt.Grpc.Extensions;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

[Authorize]
public class IpFiltersService : IpFilters.IpFiltersBase
{
    private readonly IMediator _mediator;

    public IpFiltersService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<IpFiltersResponse> GetIpFilters(GetIpFiltersRequest request, ServerCallContext context)
    {
        var userId = context.GetAuthClaimValue(AuthClaims.UserId);

        var ipFiltersResult = await _mediator.Send(new GetIpFiltersByUserIdQuery
        {
            UserId = userId
        });

        var ipFilters = ipFiltersResult.Value.Select(i => new IpFilterModel
        {
            Ip = i.Ip,
            FilterAction = (int)i.FilterAction
        }).ToList();

        return new IpFiltersResponse
        {
            IpFilters = { ipFilters }
        };
    }
}