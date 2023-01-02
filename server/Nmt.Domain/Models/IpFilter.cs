using Nmt.Domain.Enums;

namespace Nmt.Domain.Models;

public class IpFilter
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public uint Ip { get; set; }
    public IpFilterAction FilterAction { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}