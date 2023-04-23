namespace ArkProjects.EHentai.Api.Models;

public class EHTrackerOverview
{
    public double Uploaded { get; set; }
    public double Downloaded { get; set; }
    public long TorrentCompletes { get; set; }
    public long GalleryCompletes { get; set; }
    public double UpDownRatio { get; set; }
    public long SeedMinutes { get; set; }
}