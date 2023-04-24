using ArkProjects.XUnit.Json;

namespace ArkProjects.EHentai.Api.Tests;

public class ParsingTestData<T> : XUnitJsonSerializable
{
    public T ExpectedResult { get; set; }

    public string HtmlPath { get; set; }

    [XUnitJsonFileName] 
    public string File { get; set; }

    [XUnitJsonCaseIndex] 
    public int Idx { get; set; }

    public override string ToString()
    {
        return $"[{Idx}]{File}";
    }
}