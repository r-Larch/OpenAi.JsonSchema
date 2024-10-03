using OpenAi.JsonSchema.Tests.Models;
using OpenAi.JsonSchema.Nodes;
using Xunit.Abstractions;


namespace OpenAi.JsonSchema.Tests;

public class DefaultTests(ITestOutputHelper output) {
    [Fact]
    public void Test1()
    {
        var schema = Helper.Generate<Parent>();

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Test_Enums()
    {
        var schema = Helper.Generate<EnumClass>();

        var enums = schema.All<SchemaEnumNode>();
        Assert.NotEmpty(enums);

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Can_have_anyOf_at_root()
    {
        var schema = Helper.Generate<Child>();

        Assert.IsType<SchemaAnyOfNode>(schema.Root);

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }
}
