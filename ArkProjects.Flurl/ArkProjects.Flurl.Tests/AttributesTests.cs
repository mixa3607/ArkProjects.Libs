using ArkProjects.Flurl.Attributes;
using ArkProjects.Flurl.Converters;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ArkProjects.Flurl.Tests;

public class AttributesTests
{
    public class NameAttrClass
    {
        [FlurlQueryMember("A")] public string? RenameToA { get; set; } = "ValA";

        [FlurlQueryMember("B")] public string? RenameToB = "ValB";
    }

    [Fact]
    public void NameAttribute()
    {
        //multiple args strategy
        using (new AssertionScope())
        {
            var source = new NameAttrClass();
            var except = new Dictionary<string, string?>()
            {
                { "A", "ValA" },
                { "B", "ValB" },
            };

            var serializer = new FlurlQuerySerializer(new FlurlQuerySerializerSettings()
            {
                ArrayStrategy = FlurlQuerySerializerArrayStrategy.MultipleArgs
            });
            var result = serializer.Serialize(source);
            result.Should().Equal(except);
        }
    }

    private class ObjToXConverter : FlurlMemberConverter
    {
        public override string? Convert(object? value, FlurlQuerySerializer serializer) => "X";

        public override bool CanConvert(Type objectType) => true;
    }

    public class ConverterAttrClass
    {
        [FlurlQueryMemberConvert(typeof(ObjToXConverter))]
        public string? A { get; set; } = "ValA";

        public string? B = "ValB";
    }

    [Fact]
    public void ConverterAttribute()
    {
        //multiple args strategy
        using (new AssertionScope())
        {
            var source = new ConverterAttrClass();
            var except = new Dictionary<string, string?>()
            {
                { "A", "X" },
                { "B", "ValB" },
            };

            var serializer = new FlurlQuerySerializer(new FlurlQuerySerializerSettings()
            {
                ArrayStrategy = FlurlQuerySerializerArrayStrategy.MultipleArgs
            });
            var result = serializer.Serialize(source);
            result.Should().Equal(except);
        }
    }
}