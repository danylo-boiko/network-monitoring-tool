namespace Nmt.Core.Models;

public class BlockedIp
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public uint Ip { get; set; }
    public string Reason { get; set; }
    public DateTime BlockedAt { get; set; }
}