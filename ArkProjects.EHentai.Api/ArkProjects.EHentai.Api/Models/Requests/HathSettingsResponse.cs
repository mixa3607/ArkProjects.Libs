using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ArkProjects.EHentai.Api.Models.Requests;

public class HathSettingsResponse
{
    public long ClientId { get; set; } = -1;
    public string? ClientName { get; set; }
    public string? ClientKey { get; set; }

    public IReadOnlyDictionary<StaticRangeGroupType, int>? StaticRangeGroups { get; set; }

    public static HathSettingsResponse Parse(HtmlDocument doc)
    {
        var culture = CultureInfo.InvariantCulture;

        var response = new HathSettingsResponse();

        var tables = doc.DocumentNode.SelectNodes("//*/table");

        {
            var table = tables.FirstOrDefault(x => x.SelectSingleNode(".//tr/td[text()='Client ID:']") != null);
            var rowCells = table!.ChildNodes.First(x => x.Name == "tr").ChildNodes.Where(x => x.Name == "td").ToArray();
            var clientIdRaw = rowCells[1].InnerText;
            var clientKeyRaw = rowCells[3].InnerText;

            response.ClientId = long.Parse(clientIdRaw.Trim(), culture);
            response.ClientKey = clientKeyRaw.Trim();
        }

        {

            var table = doc.DocumentNode.SelectSingleNode("//*/table[@class='infot']");
            var rows = table.SelectNodes(".//tr");
            foreach (var row in rows)
            {
                if (row.SelectNodes(".//td[@class='infota']").FirstOrDefault()?.InnerText.Contains("Client Name") == true)
                {
                    var text = row.SelectNodes(".//td[@class='infotv']/p/span").First().InnerText;
                    response.ClientName = text?.Trim();
                }

                if (row.SelectNodes(".//td[@class='infota']").FirstOrDefault()?.InnerText.Contains("Reset Static Ranges") == true)
                {
                    var text = row.SelectNodes(".//td[@class='infotv']/p[1]").First().InnerText;
                    {
                        var match = Regex.Match(text, "(?<name>\\w+) = (?<value>\\d+)");
                        var groups = new Dictionary<StaticRangeGroupType, int>(5);
                        response.StaticRangeGroups = groups;
                        while (match.Success)
                        {
                            groups[match.Groups["name"].Value switch
                            {
                                "P1" => StaticRangeGroupType.Priority1,
                                "P2" => StaticRangeGroupType.Priority2,
                                "P3" => StaticRangeGroupType.Priority3,
                                "P4" => StaticRangeGroupType.Priority4,
                                "HC" => StaticRangeGroupType.HighCapacity,
                                _ => StaticRangeGroupType.Unknown
                            }] = int.Parse(match.Groups["value"].Value);

                            match = match.NextMatch();
                        }
                    }
                }
            }
        }

        return response;
    }
}