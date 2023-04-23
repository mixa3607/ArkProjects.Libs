using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ArkProjects.EHentai.Api.Models.Requests;

public class HathSettingsResponse
{
    public long ClientId { get; set; }
    public string? ClientKey { get; set; }

    public int StaticRanges { get; set; }

    public static HathSettingsResponse Parse(HtmlDocument doc)
    {
        var culture = CultureInfo.InvariantCulture;

        var response = new HathSettingsResponse();

        var tables = doc.DocumentNode.SelectNodes("//*/table");

        {
            var table = tables.FirstOrDefault(x => x.SelectSingleNode(".//tr/td[text()='Client ID#:']") != null);
            var rowCells = table!.ChildNodes.First(x => x.Name == "tr").ChildNodes.Where(x => x.Name == "td").ToArray();
            var clientIdRaw = rowCells[1].InnerText;
            var clientKeyRaw = rowCells[3].InnerText;

            response.ClientId = long.Parse(clientIdRaw.Trim(), culture);
            response.ClientKey = clientKeyRaw.Trim();
        }

        {
            var table = tables.FirstOrDefault(x => x.SelectSingleNode(".//tr/td/div[text()='Port for Incoming Connections']") != null);
            var rows = table!.ChildNodes.Where(x => x.Name == "tr").ToArray();
            {
                var rowCells = rows[10].ChildNodes.Where(x => x.Name == "td").ToArray();
                var staticRangesRaw = rowCells[1].InnerText;

                response.StaticRanges = int.Parse(Regex.Match(staticRangesRaw, "\\d+").Value, culture);
            }
        }

        return response;
    }
}