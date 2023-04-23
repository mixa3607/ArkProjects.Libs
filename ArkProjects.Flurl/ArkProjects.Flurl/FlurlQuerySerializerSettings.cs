using System.Collections.Generic;
using ArkProjects.Flurl.Converters;

namespace ArkProjects.Flurl;

public class FlurlQuerySerializerSettings
{
    public FlurlQuerySerializerArrayStrategy ArrayStrategy { get; set; }
    public List<FlurlMemberConverter> Converters { get; set; } = new();
}