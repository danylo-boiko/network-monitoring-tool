using Grpc.Core;
using MediatR;
using Nmt.Core.CQRS.Queries.IpFilters.GetIpFiltersByUserId;
using Nmt.Domain.Consts;
using Nmt.Domain.Enums;
using Nmt.Grpc.Attributes;
using Nmt.Grpc.Extensions;
using Nmt.Grpc.Protos;

namespace Nmt.Grpc.Services;

public class IpFiltersService : IpFilters.IpFiltersBase
{
    private readonly IMediator _mediator;

    public IpFiltersService(IMediator mediator)
    {
        _mediator = mediator;
    }

    [PermissionsAuthorize(Permission.IpFiltersRead)]
    public override async Task<IpFiltersResponse> GetIpFilters(GetIpFiltersRequest request, ServerCallContext context)
    {
        var userId = context.GetAuthClaimValue(AuthClaims.UserId);

        var ipFilters = await _mediator.Send(new GetIpFiltersByUserIdQuery
        {
            UserId = userId
        }, context.CancellationToken);

        var ipFilterModels = ipFilters.Select(i => new IpFilterModel
        {
            Ip = i.Ip,
            FilterAction = (int)i.FilterAction
        }).ToList();

        return new IpFiltersResponse
        {
            IpFilters = { ipFilterModels }
        };
    }
}