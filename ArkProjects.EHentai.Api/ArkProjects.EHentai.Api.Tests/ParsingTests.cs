using ArkProjects.EHentai.Api.Models.Requests;
using ArkProjects.XUnit.Json;
using HtmlAgilityPack;
using FluentAssertions;
using Newtonsoft.Json;

namespace ArkProjects.EHentai.Api.Tests
{
    public class ParsingTests
    {
        [Theory]
        [JsonData("./files/{class}/{method}_1.json")]
        [JsonData("./files/{class}/{method}_2.json")]
        [JsonData("./files/{class}/{method}_3.json")]
        public void HathPerks(ParsingTestData<HathPerksResponse> data)
        {
            var doc = new HtmlDocument();
            doc.Load(data.HtmlPath);
            var resp = HathPerksResponse.Parse(doc);
            resp.Should().BeEquivalentTo(data.ExpectedResult);
        }

        [Theory]
        [JsonData("./files/{class}/{method}_3.json")]
        public void HathStatus(ParsingTestData<HathStatusResponse> data)
        {
            var doc = new HtmlDocument();
            doc.Load(data.HtmlPath);
            var resp = HathStatusResponse.Parse(doc);
            var j = JsonConvert.SerializeObject(resp, Formatting.Indented);
            resp.Should().BeEquivalentTo(data.ExpectedResult);
        }
    }
}