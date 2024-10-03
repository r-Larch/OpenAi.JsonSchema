using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Generator.Abstractions;

public interface ISchemaBuilder {
    public SchemaNode BuildSchema(JsonType type, SchemaBuildContext context);
}

public interface IObjectSchemaBuilder : ISchemaBuilder {
}

public interface IArraySchemaBuilder : ISchemaBuilder {
}

public interface IValueSchemaBuilder : ISchemaBuilder {
}
