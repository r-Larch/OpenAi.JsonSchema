using System.Text.Json.Nodes;
using OpenAi.JsonSchema.Nodes.Abstractions;
using OpenAi.JsonSchema.Serialization;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaRootNode(SchemaNode Root, SchemaDefinitionNode Definitions, JsonSchemaOptions Options) : SchemaNode {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);

    public JsonNode ToJsonNode()
    {
        var serializer = new JsonSchemaSerializer(Options);
        return serializer.Serialize(this);
    }

    public string ToJson()
    {
        return ToJsonNode().ToJsonString(Options.JsonSerializerOptions);
    }
}
