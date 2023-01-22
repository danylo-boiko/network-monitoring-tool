namespace Nmt.Core.CQRS.Commands.Packets.CreatePackets;

public class PacketsAggregate
{
    public int CountOfUniqueIps { get; set; }
    public float AverageCountOfPacketsPerIp { get; set; }

    public bool HasAnomaly(PacketsAggregate previous, int comparisonMultiplier)
    {
        return CountOfUniqueIps / comparisonMultiplier > previous.CountOfUniqueIps ||
               AverageCountOfPacketsPerIp / comparisonMultiplier > previous.AverageCountOfPacketsPerIp;
    }

    public static string GetCacheKey(Guid deviceId)
    {
        return $"PacketsAggregate_{deviceId}";
    }
}