using OpenAi.JsonSchema.Tests.Models;
using OpenAi.JsonSchema.Serialization;
using Xunit.Abstractions;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Tests;

public class DescriptionTests(ITestOutputHelper output) {
    [Fact]
    public void Can_have_description()
    {
        var options = new JsonSchemaOptions(Helper.JsonOptions);

        var schema = Helper.Generate<Document>(options);

        foreach (var node in schema.All<SchemaNode>().Where(_ => _ is not SchemaRootNode)) {
            output.WriteLine($"{node.GetType().Name}: {node.Description}");
            switch (node) {
                case SchemaRootNode or SchemaDefinitionNode or SchemaValueNode { Type: "null" }:
                    Assert.True(true);
                    break;
                case SchemaRefNode:
                    Assert.True(node
                        is SchemaRefNode { Description.Length: > 0 }
                        or SchemaRefNode { Ref.Value.Description.Length: > 0 }
                    );
                    break;
                default:
                    Assert.NotNull(node.Description);
                    break;
            }
        }

        var json = schema.ToJson();
        output.WriteLine(json);
        Assert.NotNull(json);
    }
}
