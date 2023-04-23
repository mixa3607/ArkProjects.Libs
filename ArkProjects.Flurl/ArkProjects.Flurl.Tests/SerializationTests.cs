using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ArkProjects.Flurl.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void ListSerialize()
        {
            //empty
            using (new AssertionScope())
            {
                var source = new List<string>();
                var except = new Dictionary<string, string?>();

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            //strings
            using (new AssertionScope())
            {
                var source = new List<string?>()
                {
                    "Abc", null, "123"
                };
                var except = new Dictionary<string, string?>()
                {
                    { "Abc", null },
                    { "123", null }
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            using (new AssertionScope())
            {
                var source = new List<string?>()
                {
                    "Abc=55z", null, "**75123"
                };
                var except = new Dictionary<string, string?>()
                {
                    { "Abc=55z", null },
                    { "**75123", null }
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            //kv list
            using (new AssertionScope())
            {
                var source = new List<KeyValuePair<object?, object?>?>()
                {
                    new("aaa", "bbb"),
                    null,
                    new(null, 777),
                    new(888, null),
                    new("fff", 555),
                };
                var except = new Dictionary<string, string?>()
                {
                    { "aaa", "bbb" },
                    { "888", null },
                    { "fff", "555" }
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            //tuple list
            using (new AssertionScope())
            {
                var source = new List<(object? k, object? v)?>()
                {
                    new("aaa", "bbb"),
                    null,
                    new(null, 777),
                    new(888, null),
                    new("fff", 555),
                };
                var except = new Dictionary<string, string?>()
                {
                    { "aaa", "bbb" },
                    { "888", null },
                    { "fff", "555" }
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            //anon objs like kv list
            using (new AssertionScope())
            {
                var source = new List<object?>()
                {
                    new { Key = "aaa", Value = "bbb" },
                    null,
                    new { Key = (string?)null, Value = "asas" },
                    new { Key = 888, Value = (string?)null },
                    new { Key = "fff", Value = "555" },
                };
                var except = new Dictionary<string, string?>()
                {
                    { "aaa", "bbb" },
                    { "888", null },
                    { "fff", "555" }
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }
        }

        [Fact]
        public void StringSerialize()
        {
            //empty
            using (new AssertionScope())
            {
                var source = "";
                var except = new Dictionary<string, string?>();

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            using (new AssertionScope())
            {
                var source = "Abc=&&123=";
                var except = new Dictionary<string, string?>()
                {
                    { "Abc", "" },
                    { "123", "" }
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }
        }

        [Fact]
        public void ObjectSerialize()
        {
            //empty
            using (new AssertionScope())
            {
                var source = new { };
                var except = new Dictionary<string, string?>();

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }

            //obj
            using (new AssertionScope())
            {
                var source = new { P1 = (string?)null, P2 = "aaa", P3 = 777 };
                var except = new Dictionary<string, string?>()
                {
                    { "P1", null },
                    { "P2", "aaa" },
                    { "P3", "777" },
                };

                var serializer = new FlurlQuerySerializer();
                var result = serializer.Serialize(source);
                result.Should().Equal(except);
            }
        }
    }
}