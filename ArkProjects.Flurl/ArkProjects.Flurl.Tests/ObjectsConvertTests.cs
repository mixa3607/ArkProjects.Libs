using ArkProjects.Flurl.Converters;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ArkProjects.Flurl.Tests;

public class ObjectsConvertTests
{
    [Fact]
    public void ListConv()
    {
        //multiple args strategy
        using (new AssertionScope())
        {
            var source = new List<KeyValuePair<string, object?>?>()
            {
                new("1", new[] { 'a', 'b', 'c' }),
            };
            var except = new Dictionary<string, string?>()
            {
                { "1[0]", "a" },
                { "1[1]", "b" },
                { "1[2]", "c" },
            };

            var serializer = new FlurlQuerySerializer(new FlurlQuerySerializerSettings()
            {
                ArrayStrategy = FlurlQuerySerializerArrayStrategy.MultipleArgs
            });
            var result = serializer.Serialize(source);
            result.Should().Equal(except);
        }

        //join args strategy
        using (new AssertionScope())
        {
            var source = new List<KeyValuePair<string, object?>?>()
            {
                new("1", new[] { 'a', 'b', 'c' }),
            };
            var except = new Dictionary<string, string?>()
            {
                { "1", "a,b,c" },
            };

            var serializer = new FlurlQuerySerializer(new FlurlQuerySerializerSettings()
            {
                ArrayStrategy = FlurlQuerySerializerArrayStrategy.SingleArgJoin
            });
            var result = serializer.Serialize(source);
            result.Should().Equal(except);
        }
    }

    private class BoolArrToXConverter : FlurlMemberConverter<IEnumerable<bool>>
    {
        public override string? Convert(IEnumerable<bool>? value, FlurlQuerySerializer serializer)
        {
            return "X";
        }
    }

    [Fact]
    public void CustomConv()
    {
        //allow per element apply
        using (new AssertionScope())
        {
            var source = new List<KeyValuePair<string, object?>?>()
            {
                new("1", new[] { true, false, true }),
            };
            var except = new Dictionary<string, string?>()
            {
                { "1[0]", "True" },
                { "1[1]", "False" },
                { "1[2]", "True" },
            };

            var serializer = new FlurlQuerySerializer(new FlurlQuerySerializerSettings()
            {
                ArrayStrategy = FlurlQuerySerializerArrayStrategy.MultipleArgs,
                Converters = new List<FlurlMemberConverter>() { new FlurlMemberConvertBoolToInt() }
            });
            var result = serializer.Serialize(source);
            result.Should().Equal(except);
        }

        //allow element apply
        using (new AssertionScope())
        {
            var source = new List<KeyValuePair<string, object?>?>()
            {
                new("1", new[] { true, false, true }),
            };
            var except = new Dictionary<string, string?>()
            {
                { "1", "X" }
            };

            var serializer = new FlurlQuerySerializer(new FlurlQuerySerializerSettings()
            {
                ArrayStrategy = FlurlQuerySerializerArrayStrategy.MultipleArgs,
                Converters = new List<FlurlMemberConverter>() { new BoolArrToXConverter() }
            });
            var result = serializer.Serialize(source);
            result.Should().Equal(except);
        }
    }
}