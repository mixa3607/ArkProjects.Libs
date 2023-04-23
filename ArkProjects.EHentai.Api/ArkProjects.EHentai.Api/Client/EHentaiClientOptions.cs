using ArkProjects.EHentai.Api.Proxy;

namespace ArkProjects.EHentai.Api.Client;

public class EHentaiClientOptions
{
    public string? SessionId { get; set; }
    public string? PassHash { get; set; }
    public string? MemberId { get; set; }
    public string? Igneous { get; set; }
    public EHentaiSiteType SiteType { get; set; }
    public string? OverrideSiteAddress { get; set; }
    public string? OverrideUserAgent { get; set; }
    public WebProxyOptions Proxy { get; set; } = new();
}