using System.Text.Json.Nodes;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Serialization;

public class JsonSchemaSerializer(JsonSchemaOptions options) {
    private readonly SchemaSerializer _serializer = new(options);
    public JsonNode Serialize(SchemaRootNode schema) => _serializer.Transform(schema);
}
