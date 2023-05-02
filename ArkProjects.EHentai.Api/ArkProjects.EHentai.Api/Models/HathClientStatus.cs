namespace ArkProjects.EHentai.Api.Models;

public enum HathClientStatus : byte
{
    Unknown = 0,
    Timeout = 100,
    Offline = 150,
    Online = 200,
}