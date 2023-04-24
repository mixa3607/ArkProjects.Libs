using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ArkProjects.EHentai.Api.Models.Requests;

public class HathPerksResponse
{
    public double Hath { get; set; }

    public static HathPerksResponse Parse(HtmlDocument doc)
    {
        var culture = CultureInfo.InvariantCulture;

        var response = new HathPerksResponse();

        var hathRaw = doc.DocumentNode
            .SelectNodes("//*/p")
            .First(x => x.InnerText.Contains("You currently have "))
            .InnerText;
        response.Hath = double.Parse(Regex.Match(hathRaw, "\\d+(\\.\\d+)*").Value, culture);

        return response;
    }
}