using ArkProjects.XUnit.Json;

namespace ArkProjects.EHentai.Api.Tests;

public class ParsingTestData<T> : XUnitJsonSerializable
{
    public required T ExpectedResult { get; set; }

    public required string HtmlPath { get; set; }

    [XUnitJsonFileName] 
    public required string File { get; set; }

    [XUnitJsonCaseIndex] 
    public int Idx { get; set; }

    public override string ToString()
    {
        return $"[{Idx}]{File}";
    }
}