using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Serialization;


namespace OpenAi.JsonSchema.Generator.Abstractions;

public class SchemaBuildContext(ISchemaBuilder builder, JsonSchemaOptions options) {
    public DefinitionCollection Definitions { get; } = new();
    public JsonSchemaOptions Options { get; } = options;

    public SchemaNode Generate(JsonType type)
    {
        return builder.BuildSchema(type, this);
    }
}
