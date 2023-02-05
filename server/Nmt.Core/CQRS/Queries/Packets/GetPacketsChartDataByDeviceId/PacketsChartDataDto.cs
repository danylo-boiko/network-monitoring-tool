namespace Nmt.Core.CQRS.Queries.Packets.GetPacketsChartDataByDeviceId;

public class PacketsChartDataDto
{
    public IDictionary<string, IList<int>> Series { get; set; }
    public IList<string> Categories { get; set; }

    public PacketsChartDataDto()
    {
        Series = new Dictionary<string, IList<int>>();
        Categories = new List<string>();
    }
}