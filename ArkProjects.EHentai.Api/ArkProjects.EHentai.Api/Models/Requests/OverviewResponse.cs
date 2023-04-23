using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ArkProjects.EHentai.Api.Models.Requests;

public class OverviewResponse
{
    public EHTrackerOverview EhTracker { get; set; } = new();
    public TotalGpGainedOverview TotalGpGained { get; set; } = new();

    public static OverviewResponse Parse(HtmlDocument doc)
    {
        var culture = CultureInfo.InvariantCulture;
        var response = new OverviewResponse();

        var tables = doc.DocumentNode.SelectNodes("//*/table");

        {
            var table = tables.First(x => x.SelectSingleNode(".//tr/td[text()='up/down ratio']") != null);
            var rows = table.SelectNodes(".//tr");
            {
                var cells = rows[0].SelectNodes(".//td");
                var uploadedRaw = cells[0].InnerText;
                var downloadedRaw = cells[2].InnerText;
                var ratioRaw = cells[4].InnerText;

                response.EhTracker.Uploaded = double.Parse(Regex.Match(uploadedRaw.Replace(",", ""), "\\d+\\.\\d+").Value, culture);
                response.EhTracker.Downloaded = double.Parse(Regex.Match(downloadedRaw.Replace(",", ""), "\\d+\\.\\d+").Value, culture);
                response.EhTracker.UpDownRatio = double.Parse(Regex.Match(ratioRaw.Replace(",", ""), "\\d+\\.\\d+").Value, culture);
            }
            {
                var cells = rows[1].SelectNodes(".//td");
                var torrentsRaw = cells[0].InnerText;
                var galleriesRaw = cells[2].InnerText;
                var seedminsRaw = cells[4].InnerText;

                response.EhTracker.TorrentCompletes = long.Parse(Regex.Match(torrentsRaw.Replace(",", ""), "\\d+").Value, culture);
                response.EhTracker.GalleryCompletes = long.Parse(Regex.Match(galleriesRaw.Replace(",", ""), "\\d+").Value, culture);
                response.EhTracker.SeedMinutes = long.Parse(Regex.Match(seedminsRaw.Replace(",", ""), "\\d+").Value, culture);
            }
        }
        {
            var table = tables.First(x => x.SelectSingleNode(".//tr/td[text()='GP from gallery visits']") != null);
            var rows = table.SelectNodes(".//tr");
            var gvRaw = rows[0].SelectNodes(".//td")[0].InnerText;
            var torrentsRaw = rows[1].SelectNodes(".//td")[0].InnerText;
            var archivesRaw = rows[2].SelectNodes(".//td")[0].InnerText;
            var hathRaw = rows[3].SelectNodes(".//td")[0].InnerText;

            response.TotalGpGained.FromGalleryVisits = long.Parse(Regex.Match(gvRaw.Replace(",", ""), "\\d+").Value, culture);
            response.TotalGpGained.FromTorrentCompletions= long.Parse(Regex.Match(torrentsRaw.Replace(",", ""), "\\d+").Value, culture);
            response.TotalGpGained.FromArchiveDownloads = long.Parse(Regex.Match(archivesRaw.Replace(",", ""), "\\d+").Value, culture);
            response.TotalGpGained.FromHath = long.Parse(Regex.Match(hathRaw.Replace(",", ""), "\\d+").Value, culture);
        }

        return response;
    }
}