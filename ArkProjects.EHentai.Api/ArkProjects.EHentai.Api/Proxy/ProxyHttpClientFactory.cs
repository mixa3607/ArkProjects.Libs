using System.Net;
using Flurl.Http.Configuration;

namespace ArkProjects.EHentai.Api.Proxy;

public class ProxyHttpClientFactory : DefaultHttpClientFactory
{
    private readonly WebProxyOptions _options;

    public ProxyHttpClientFactory(WebProxyOptions _options)
    {
        this._options = _options;
    }

    public override HttpMessageHandler CreateMessageHandler()
    {
        if (_options.Address == null) 
            return new HttpClientHandler();

        var proxy = new WebProxy
        {
            Address = _options.Address,
            BypassProxyOnLocal = _options.BypassLocal,
            Credentials = new NetworkCredential(_options.UserName, _options.Password, _options.Domain)
        };
        return new HttpClientHandler()
        {
            Proxy = proxy,
            UseProxy = true
        };
    }
}