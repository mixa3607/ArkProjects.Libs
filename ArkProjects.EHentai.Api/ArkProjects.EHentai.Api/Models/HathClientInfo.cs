namespace ArkProjects.EHentai.Api.Models;

public class HathClientInfo
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public HathClientStatus Status { get; set; }
    public DateOnly Created { get; set; }
    public DateTime LastSeen { get; set; }
    public long FilesServed { get; set; }
    public string? ClientIp { get; set; }
    public int Port { get; set; }
    public string? Version { get; set; }
    public long MaxSpeed { get; set; }
    public int Trust { get; set; }
    public int Quality { get; set; }
    public double Hitrate { get; set; }
    public double Hathrate { get; set; }
    public string? Country { get; set; }
}