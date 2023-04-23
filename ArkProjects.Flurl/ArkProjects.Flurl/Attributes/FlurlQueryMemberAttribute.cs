using System;

namespace ArkProjects.Flurl.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FlurlQueryMemberAttribute : Attribute
{
    public string? Name { get; set; }

    public FlurlQueryMemberAttribute()
    {
    }

    public FlurlQueryMemberAttribute(string? name)
    {
        Name = name;
    }
}