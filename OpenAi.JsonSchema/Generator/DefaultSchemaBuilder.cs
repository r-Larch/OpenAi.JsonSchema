using OpenAi.JsonSchema.Generator.Abstractions;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Generator;

public class DefaultSchemaBuilder(
    SchemaBuilderProvider? provider = null
) : ISchemaBuilder {
    private SchemaBuilderProvider Provider => provider ?? SchemaBuilderProvider.Default;

    public virtual SchemaNode BuildSchema(JsonType type, SchemaBuildContext context)
    {
        return type.Kind switch {
            JsonTypeKind.Object => Provider.Objects.BuildSchema(type, context),
            JsonTypeKind.Array => Provider.Arrays.BuildSchema(type, context),
            JsonTypeKind.Value => Provider.Values.BuildSchema(type, context),
            _ => throw new ArgumentOutOfRangeException(nameof(type.Kind), type.Kind, null)
        };
    }
}
