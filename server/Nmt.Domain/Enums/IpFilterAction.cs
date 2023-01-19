namespace Nmt.Domain.Enums;

public enum IpFilterAction : byte
{
    PassWithoutCollecting = 1,
    Drop = 2,
    DropWithoutCollecting = 3
}