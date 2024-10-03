using System.Text.Json;
using OpenAi.JsonSchema.Generator.Abstractions;
using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Serialization;
using OpenAi.JsonSchema.Validation;


namespace OpenAi.JsonSchema.Generator;

public class DefaultSchemaGenerator(ISchemaBuilder? builder = null) : ISchemaGenerator {
    private ISchemaBuilder Builder { get; } = builder ?? new DefaultSchemaBuilder();

    public virtual SchemaRootNode Generate(Type type, JsonSerializerOptions? options = null)
    {
        return Generate(type, new JsonSchemaOptions(options));
    }

    public virtual SchemaRootNode Generate(Type type, JsonSchemaOptions? options = null)
    {
        options ??= JsonSchemaOptions.Default;

        var resolver = new JsonTypeResolver(options.JsonSerializerOptions);
        var context = new SchemaBuildContext(Builder, options);

        var info = resolver.GetTypeInfo(type);
        var root = Builder.BuildSchema(info, context);

        var definitions = new SchemaDefinitionNode(context.Definitions.Values);
        var schema = new SchemaRootNode(root, definitions, options);

        if (options.Transformer is { } transformer) {
            schema = transformer.Transform(schema) as SchemaRootNode ?? throw new Exception("Schema Transformer must return a SchemaRootNode!");
        }

        if (options.Validator is { } validator) {
            var result = validator.Validate(schema);
            if (!result.Valid) throw new ValidationException(result.Errors);
        }

        return schema;
    }

    public SchemaRootNode Generate<T>(JsonSerializerOptions? options = null) => Generate(typeof(T), options);
    public SchemaRootNode Generate<T>(JsonSchemaOptions? options = null) => Generate(typeof(T), options);
}
