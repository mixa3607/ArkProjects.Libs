using ArkProjects.EHentai.Api.Models.Requests;
using ArkProjects.XUnit.Json;
using HtmlAgilityPack;
using FluentAssertions;

namespace ArkProjects.EHentai.Api.Tests
{
    public class ParsingTests
    {
        [Theory]
        [JsonData("./files/{class}/{method}_1.json")]
        [JsonData("./files/{class}/{method}_2.json")]
        public void HathPerks(ParsingTestData<HathPerksResponse> data)
        {
            var doc = new HtmlDocument();
            doc.Load(data.HtmlPath);
            var resp = HathPerksResponse.Parse(doc);
            resp.Should().BeEquivalentTo(data.ExpectedResult);
        }

        [Theory]
        [JsonData("./files/{class}/{method}_1.json")]
        [JsonData("./files/{class}/{method}_2.json")]
        public void HathStatus(ParsingTestData<HathStatusResponse> data)
        {
            var doc = new HtmlDocument();
            doc.Load(data.HtmlPath);
            var resp = HathStatusResponse.Parse(doc);
            resp.Should().BeEquivalentTo(data.ExpectedResult);
        }
    }
}