using OpenAi.JsonSchema.Generator.Abstractions;


namespace OpenAi.JsonSchema.Generator;

public class SchemaBuilderProvider {
    public static SchemaBuilderProvider Default = new();
    public IObjectSchemaBuilder Objects { get; set; } = new DefaultObjectSchemaBuilder();
    public IArraySchemaBuilder Arrays { get; set; } = new DefaultArraySchemaBuilder();
    public IValueSchemaBuilder Values { get; set; } = new DefaultValueSchemaBuilder();
}
