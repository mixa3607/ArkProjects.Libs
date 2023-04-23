using ArkProjects.EHentai.Api.Models.Requests;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ArkProjects.EHentai.Api.Client;

public partial class EHentaiClient
{
    public class MyHomeApi
    {
        private readonly EHentaiClient _client;
        private readonly ILogger<EHentaiClient> _logger;

        public MyHomeApi(EHentaiClient client, ILogger<EHentaiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<EHentaiClientResponse<HathStatusResponse>> GetHathStatusAsync(CancellationToken ct = default)
        {
            if (_client._options.SiteType != EHentaiSiteType.EHentai)
                throw new NotSupportedException("This api available only for e-hentai");

            var fReq = _client.BuildBaseRequest("/hentaiathome.php");
            var fResp = await _client.MakeRequest(fReq, HttpMethod.Get, ct: ct);
            var respStr = await fResp.ResponseMessage.Content.ReadAsStringAsync(ct);

            var doc = new HtmlDocument();
            doc.LoadHtml(respStr);

            return new EHentaiClientResponse<HathStatusResponse>()
            {
                Body = HathStatusResponse.Parse(doc),
                RawStringBody = respStr
            };
        }

        public async Task<EHentaiClientResponse<HathSettingsResponse>> GetHathSettingsAsync(long clientId,
            CancellationToken ct = default)
        {
            if (_client._options.SiteType != EHentaiSiteType.EHentai)
                throw new NotSupportedException("This api available only for e-hentai");

            var args = new 
            {
                cid = clientId,
                act = "settings"
            };

            var fReq = _client.BuildBaseRequest("/hentaiathome.php", args);
            var fResp = await _client.MakeRequest(fReq, HttpMethod.Get, ct: ct);
            var respStr = await fResp.ResponseMessage.Content.ReadAsStringAsync(ct);

            var doc = new HtmlDocument();
            doc.LoadHtml(respStr);

            return new EHentaiClientResponse<HathSettingsResponse>()
            {
                Body = HathSettingsResponse.Parse(doc),
                RawStringBody = respStr
            };
        }

        public async Task<EHentaiClientResponse<HathScheduleResponse>> GetHathScheduleAsync(int clientId,
            CancellationToken ct = default)
        {
            if (_client._options.SiteType != EHentaiSiteType.EHentai)
                throw new NotSupportedException("This api available only for e-hentai");

            var args = new
            {
                cid = clientId,
                act = "schedule"
            };

            var fReq = _client.BuildBaseRequest("/hentaiathome.php", args);
            var fResp = await _client.MakeRequest(fReq, HttpMethod.Get, ct: ct);
            var respStr = await fResp.ResponseMessage.Content.ReadAsStringAsync(ct);

            var doc = new HtmlDocument();
            doc.LoadHtml(respStr);

            return new EHentaiClientResponse<HathScheduleResponse>()
            {
                Body = HathScheduleResponse.Parse(doc),
                RawStringBody = respStr
            };
        }

        public async Task<EHentaiClientResponse<HathPerksResponse>> GetHathPerksAsync(CancellationToken ct = default)
        {
            if (_client._options.SiteType != EHentaiSiteType.EHentai)
                throw new NotSupportedException("This api available only for e-hentai");
            
            var fReq = _client.BuildBaseRequest("/hathperks.php", null);
            var fResp = await _client.MakeRequest(fReq, HttpMethod.Get, ct: ct);
            var respStr = await fResp.ResponseMessage.Content.ReadAsStringAsync(ct);

            var doc = new HtmlDocument();
            doc.LoadHtml(respStr);

            return new EHentaiClientResponse<HathPerksResponse>()
            {
                Body = HathPerksResponse.Parse(doc),
                RawStringBody = respStr
            };
        }

        public async Task<EHentaiClientResponse<OverviewResponse>> GetOverviewAsync(CancellationToken ct = default)
        {
            if (_client._options.SiteType != EHentaiSiteType.EHentai)
                throw new NotSupportedException("This api available only for e-hentai");

            var fReq = _client.BuildBaseRequest("/home.php", null);
            var fResp = await _client.MakeRequest(fReq, HttpMethod.Get, ct: ct);
            var respStr = await fResp.ResponseMessage.Content.ReadAsStringAsync(ct);

            var doc = new HtmlDocument();
            doc.LoadHtml(respStr);

            return new EHentaiClientResponse<OverviewResponse>()
            {
                Body = OverviewResponse.Parse(doc),
                RawStringBody = respStr
            };
        }
    }
}