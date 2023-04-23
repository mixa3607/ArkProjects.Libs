namespace ArkProjects.EHentai.Api.Client;

public class EHentaiClientResponse<T>
{
    public string? RawStringBody { get; set; }
    public T? Body { get; set; }
}