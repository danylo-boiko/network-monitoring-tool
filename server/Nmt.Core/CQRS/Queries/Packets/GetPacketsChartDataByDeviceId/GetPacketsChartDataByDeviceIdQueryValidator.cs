using FluentValidation;

namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsChartDataByDeviceId;

public class GetPacketsChartDataByDeviceIdQueryValidator : AbstractValidator<GetPacketsChartDataByDeviceIdQuery>
{
    public GetPacketsChartDataByDeviceIdQueryValidator()
    {
        RuleFor(c => c.DeviceId)
            .NotEmpty()
            .WithMessage("Device id shouldn't be empty");

        RuleFor(c => c.DateRangeMode)
            .NotEmpty()
            .WithMessage("Date range mode shouldn't be empty")
            .IsInEnum()
            .WithMessage("Date range mode should be in enum range");
    }
}