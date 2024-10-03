using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Tests.Models;
using OpenAi.JsonSchema.Serialization;
using Xunit.Abstractions;


namespace OpenAi.JsonSchema.Tests;

public class EnumTests(ITestOutputHelper output) {
    [Fact]
    public void Default_enum_to_int()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptions);

        var schema = Helper.Generate<EnumClass>(options);

        var enums = schema.All<SchemaEnumNode>();
        Assert.NotEmpty(enums);

        foreach (var e in enums) {
            Assert.Equal("integer", e.Type);
            Assert.Equal(["0", "1", "2", "3", "4"], e.Values);
        }

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Enum_to_camelCase()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptionsCamelCase);

        var schema = Helper.Generate<EnumClass>(options);

        var enums = schema.All<SchemaEnumNode>();
        Assert.NotEmpty(enums);

        foreach (var e in enums) {
            Assert.Equal("string", e.Type);
            Assert.Equal(["\"default\"", "\"pascalCase\"", "\"camelCase\"", "\"snakeCase\"", "\"kebabCase\""], e.Values);
        }

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Enum_to_snake_case()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptionsSnakeCase);

        var schema = Helper.Generate<EnumClass>(options);

        var enums = schema.All<SchemaEnumNode>();
        Assert.NotEmpty(enums);

        foreach (var e in enums) {
            Assert.Equal("string", e.Type);
            Assert.Equal(["\"default\"", "\"pascal_case\"", "\"camel_case\"", "\"snake_case\"", "\"kebab_case\""], e.Values);
        }

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }
}
