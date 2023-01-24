using FluentValidation;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsByDeviceId;

public class GetPacketsByDeviceIdQueryValidator : AbstractValidator<GetPacketsByDeviceIdQuery>
{
    public GetPacketsByDeviceIdQueryValidator()
    {
        RuleFor(c => c.DeviceId)
            .NotEmpty()
            .WithMessage("Device id shouldn't be empty");
    }
}