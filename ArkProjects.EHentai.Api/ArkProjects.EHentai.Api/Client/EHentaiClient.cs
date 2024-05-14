using ArkProjects.Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace ArkProjects.EHentai.Api.Client;

public partial class EHentaiClient
{
    private readonly EHentaiClientOptions _options;
    private readonly ILogger<EHentaiClient> _logger;
    private readonly FlurlQuerySerializer _querySerializer;

    private IFlurlClient? _flurlClient;
    private CookieJar? _flurlCookies;

    public MyHomeApi MyHome { get; init; }

    public EHentaiClient(EHentaiClientOptions options, ILogger<EHentaiClient>? logger)
    {
        _options = options;
        _logger = logger ?? new NullLogger<EHentaiClient>();
        _querySerializer = new FlurlQuerySerializer();
        MyHome = new MyHomeApi(this, _logger);
    }

    internal IFlurlRequest BuildBaseRequest(string method, object? queryObj = null)
    {
        var urlBase = _options.OverrideSiteAddress ?? _options.SiteType switch
        {
            EHentaiSiteType.EHentai => "https://e-hentai.org",
            EHentaiSiteType.ExHentai => "https://exhentai.org",
            _ => throw new NotSupportedException(),
        };
        var userAgent = _options.OverrideUserAgent ?? "EHentaiNet/1.1";

        if (_flurlClient == null)
        {
            _flurlClient ??= CreateClient(urlBase);
            _flurlClient.WithHeader("User-Agent", userAgent);
        }

        if (_flurlCookies == null)
        {
            _flurlCookies = new CookieJar();

            if (_options.MemberId != null)
                _flurlCookies.AddOrReplace("ipb_member_id", _options.MemberId, urlBase);
            if (_options.MemberId != null)
                _flurlCookies.AddOrReplace("ipb_pass_hash", _options.PassHash, urlBase);
            if (_options.MemberId != null)
                _flurlCookies.AddOrReplace("ipb_session_id", _options.SessionId, urlBase);
            if (_options.Igneous != null)
                _flurlCookies.AddOrReplace("igneous", _options.SessionId, urlBase);
        }

        var fReq = _flurlClient.Request(method).WithCookies(_flurlCookies).WithAutoRedirect(false);
        if (queryObj != null)
        {
            var q = _querySerializer.Serialize(queryObj);
            fReq.SetQueryParams(q);
        }

        return fReq;
    }

    private async Task<IFlurlResponse> MakeRequest(IFlurlRequest req, HttpMethod? method = null,
        Func<HttpContent?>? contentFactory = null, CancellationToken ct = default)
    {
        return await req.SendAsync(method ?? HttpMethod.Get, contentFactory?.Invoke(), cancellationToken: ct);
    }


    private IFlurlClient CreateClient(string urlBase)
    {
        if (_options.Proxy.Address == null)
            return new FlurlClient(new HttpClient(), urlBase);

        return new FlurlClient(new HttpClient(new HttpClientHandler()
        {
            Proxy = new WebProxy
            {
                Address = _options.Proxy.Address,
                BypassProxyOnLocal = _options.Proxy.BypassLocal,
                Credentials = new NetworkCredential(
                    _options.Proxy.UserName, _options.Proxy.Password, _options.Proxy.Domain)
            },
            UseProxy = true
        }), urlBase);
    }
}