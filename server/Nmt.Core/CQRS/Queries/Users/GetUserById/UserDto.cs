using Nmt.Domain.Enums;

namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public IList<UserDeviceDto> Devices { get; set; }
    public IList<IpFilterDto> IpFilters { get; set; }
    public class UserDeviceDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public string MachineSpecificStamp { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class IpFilterDto
    {
        public Guid Id { get; set; }
        public long Ip { get; set; }
        public IpFilterAction FilterAction { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}