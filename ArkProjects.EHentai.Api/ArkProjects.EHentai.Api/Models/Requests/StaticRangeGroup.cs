namespace ArkProjects.EHentai.Api.Models.Requests;

public class StaticRangeGroup
{
    public StaticRangeGroupType Type { get; set; } = StaticRangeGroupType.Unknown;
    public int Count { get; set; } = -1;
}