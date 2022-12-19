namespace Nmt.Core.Models;

public class Device
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Hostname { get; set; }
    public string MachineSpecificStamp { get; set; }
    public DateTime CreatedAt { get; set; }
}