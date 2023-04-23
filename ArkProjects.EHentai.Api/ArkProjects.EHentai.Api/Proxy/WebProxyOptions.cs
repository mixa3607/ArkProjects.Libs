namespace ArkProjects.EHentai.Api.Proxy;

public class WebProxyOptions
{
    public Uri? Address { get; set; }
    public bool BypassLocal { get; set; } = true;
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Domain { get; set; }
}