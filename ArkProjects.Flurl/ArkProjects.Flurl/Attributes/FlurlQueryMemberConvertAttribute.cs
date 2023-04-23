using System;

namespace ArkProjects.Flurl.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class FlurlQueryMemberConvertAttribute : Attribute
{
    public Type ConverterType { get; }

    public FlurlQueryMemberConvertAttribute(Type converterType)
    {
        ConverterType = converterType;
    }
}