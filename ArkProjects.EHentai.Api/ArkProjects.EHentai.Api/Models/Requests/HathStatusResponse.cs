﻿using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ArkProjects.EHentai.Api.Models.Requests;

public class HathStatusResponse
{
    public IReadOnlyList<HathRegionInfo> Regions { get; set; } = Array.Empty<HathRegionInfo>();

    public IReadOnlyList<HathClientInfo> Clients { get; set; } = Array.Empty<HathClientInfo>();

    //public string HathVersion { get; set; }
    //public string HathBinsUrl { get; set; }
    //public string HathSrcUrl { get; set; }

    public static HathStatusResponse Parse(HtmlDocument doc)
    {
        var culture = CultureInfo.InvariantCulture;
        var response = new HathStatusResponse();

        var tables = doc.DocumentNode.SelectNodes("//*/table");

        var regionsTable = tables.FirstOrDefault(x => x.SelectSingleNode(".//tr/th[text()='H@H Region']") != null);
        var regions = new List<HathRegionInfo>(4);
        response.Regions = regions;
        foreach (var row in regionsTable!.ChildNodes.Where(x => x.Name == "tr").Skip(1))
        {
            var cells = row.ChildNodes.Where(x => x.Name == "td").ToArray();
            var regionRaw = cells[0].InnerText;
            var netLoadRaw = cells[3].InnerText;
            var missRaw = cells[4].InnerText;
            var coverageRaw = cells[5].InnerText;
            var hitrateRaw = cells[6].InnerText;
            var qualityRaw = cells[7].InnerText;

            var region = regionRaw switch
            {
                "North and South America" => HathRegionType.NorthSouthAmerica,
                "Europe and Africa" => HathRegionType.EuropeAfrica,
                "Asia and Oceania" => HathRegionType.AsiaOceania,
                "Global" => HathRegionType.Global,
                _ => HathRegionType.Unknown
            };
            var netLoad = int.Parse(Regex.Match(netLoadRaw, "\\d+").Value, culture);
            var miss = double.Parse(Regex.Match(missRaw, "\\d+\\.\\d+").Value, culture);
            var coverage = double.Parse(Regex.Match(coverageRaw, "\\d+\\.\\d+").Value, culture);
            var hitrate = double.Parse(Regex.Match(hitrateRaw, "\\d+\\.\\d+").Value, culture);
            var quality = int.Parse(Regex.Match(qualityRaw, "\\d+").Value, culture);

            var regionInfo = new HathRegionInfo()
            {
                Region = region,
                NetLoad = netLoad,
                MissRate = miss,
                Coverage = coverage,
                HitsPerGb = hitrate,
                Quality = quality
            };
            regions.Add(regionInfo);
        }

        var clientsTable = tables.FirstOrDefault(x => x.SelectSingleNode(".//tr/th[text()='Client']") != null);
        var clients = new List<HathClientInfo>(4);
        response.Clients = clients;
        foreach (var row in clientsTable!.ChildNodes.Where(x => x.Name == "tr").Skip(1))
        {
            var cells = row.ChildNodes.Where(x => x.Name == "td").ToArray();

            var nameRaw = cells[0].InnerText;
            var idRaw = cells[1].InnerText;
            var statusRaw = cells[2].InnerText;
            var createdRaw = cells[3].InnerText;
            var lastSeenRaw = cells[4].InnerText;
            var filesServedRaw = cells[5].InnerText;
            var ipRaw = cells[6].InnerText;
            var portRaw = cells[7].InnerText;
            var versionRaw = cells[8].InnerText;
            var maxSpeedRaw = cells[9].InnerText;
            var trustRaw = cells[10].InnerText;
            var qualityRaw = cells[11].InnerText;
            var hitrateRaw = cells[12].InnerText;
            var hathrateRaw = cells[13].InnerText;
            var countryRaw = cells[14].InnerText;

            var name = nameRaw.Trim();
            var id = int.Parse(Regex.Match(idRaw, "\\d+").Value, culture);
            var status = statusRaw switch
            {
                "Online" => HathClientStatus.Online,
                "Timeout" => HathClientStatus.Timeout,
                _ => HathClientStatus.Unknown
            };
            var created = DateOnly.Parse(createdRaw);
            var lastSeen = DateTime.MinValue;
            var filesServed = long.Parse(filesServedRaw.Replace(",", ""), culture);
            var ip = ipRaw.Trim();
            var port = int.Parse(Regex.Match(portRaw, "\\d+").Value, culture);
            var version = versionRaw.Trim();
            var maxSpeed = long.Parse(Regex.Match(maxSpeedRaw, "\\d+").Value, culture);
            var trust = int.Parse(Regex.Match(trustRaw, "\\d+").Value, culture);
            var quality = int.Parse(qualityRaw.Trim(), culture);
            var hitrate = double.Parse(Regex.Match(hitrateRaw, "\\d+\\.\\d+").Value, culture);
            var hathrate = double.Parse(Regex.Match(hathrateRaw, "\\d+\\.\\d+").Value, culture);
            var country = countryRaw.Trim();

            var clientInfo = new HathClientInfo()
            {
                Name = name,
                Id = id,
                Status = status,
                Created = created,
                LastSeen = lastSeen,
                FilesServed = filesServed,
                ClientIp = ip,
                Port = port,
                Version = version,
                MaxSpeed = maxSpeed,
                Trust = trust,
                Quality = quality,
                Hitrate = hitrate,
                Hathrate = hathrate,
                Country = country,
            };
            clients.Add(clientInfo);
        }

        return response;
    }
}