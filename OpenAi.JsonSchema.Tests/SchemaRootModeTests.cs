using OpenAi.JsonSchema.Tests.Models;
using OpenAi.JsonSchema.Serialization;
using Xunit.Abstractions;


namespace OpenAi.JsonSchema.Tests;

public class SchemaRootModeTests(ITestOutputHelper output) {
    [Fact]
    public void Test_RootRef()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptions) {
            SchemaRootMode = SchemaRootMode.RootRef,
        };

        var schema = Helper.Generate<Parent>(options);

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Test_RootRecursion()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptions) {
            SchemaRootMode = SchemaRootMode.RootRecursion,
        };

        var schema = Helper.Generate<Parent>(options);

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Test_RootDuplication()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptions) {
            SchemaRootMode = SchemaRootMode.RootDuplication,
        };

        var schema = Helper.Generate<Parent>(options);

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }
}
