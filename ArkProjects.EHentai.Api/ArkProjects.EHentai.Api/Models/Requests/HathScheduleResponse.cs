using System.Globalization;
using HtmlAgilityPack;

namespace ArkProjects.EHentai.Api.Models.Requests;

public class HathScheduleResponse
{
    public static HathScheduleResponse Parse(HtmlDocument doc)
    {
        var culture = CultureInfo.InvariantCulture;

        var response = new HathScheduleResponse();

        return response;
    }
}