﻿namespace ArkProjects.EHentai.Api.Models;

public class HathRegionInfo
{
    public HathRegionType Region { get; set; }
    public int NetLoad { get; set; }
    public double HitsPerSecond { get; set; }
    public double HitsPerGb { get; set; }
    public int Quality { get; set; }
}