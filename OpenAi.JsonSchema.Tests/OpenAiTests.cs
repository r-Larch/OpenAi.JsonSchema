using System.Text.Json;
using OpenAi.JsonSchema.Serialization;
using OpenAi.JsonSchema.Tests.Models;
using Xunit.Abstractions;


namespace OpenAi.JsonSchema.Tests;

public class OpenAiTests(ITestOutputHelper output) {
    [Fact]
    public void Test1()
    {
        var options = new JsonSchemaOptions(SchemaDefaults.OpenAi, Helper.JsonOptions);

        var schema = Helper.Generate<Parent>(options);

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Test_ToJsonString_with_diffrent_options()
    {
        var options = new JsonSchemaOptions(SchemaDefaults.OpenAi, Helper.JsonOptions);

        var schema = Helper.Generate<Parent>(options);

        var json = schema.ToJsonNode().ToJsonString(new JsonSerializerOptions() { WriteIndented = true });
        output.WriteLine(json);
        Assert.NotNull(json);
    }


    [Fact]
    public void Error_when_anyOf_at_root()
    {
        var options = new JsonSchemaOptions(SchemaDefaults.OpenAi, Helper.JsonOptions);

        var error = Assert.Throws<OpenAi.JsonSchema.Validation.ValidationException>(() => {
            _ = Helper.Generate<Child>(options);
        });

        output.WriteLine(error.ToString());
    }
}
