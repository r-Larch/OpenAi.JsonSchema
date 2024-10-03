using System.Text.Json;
using OpenAi.JsonSchema.Generator.Abstractions;
using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Nodes.Abstractions;
using OpenAi.JsonSchema.Validation;


namespace OpenAi.JsonSchema.Serialization;

public class JsonSchemaOptions(JsonSerializerOptions? options = null) {
    public static JsonSchemaOptions Default = new();
    public JsonSerializerOptions JsonSerializerOptions { get; } = options ?? JsonSerializerOptions.Default;

    public SchemaRootMode SchemaRootMode { get; init; } = SchemaRootMode.RootRecursion;

    public bool FormatSupported { get; init; } = true;
    public PropertyRequired? PropertyRequired { get; init; }
    public SchemaTransformer<SchemaNode>? Transformer { get; init; }
    public SchemaValidator? Validator { get; init; }

    public JsonSchemaOptions(SchemaDefaults defaults, JsonSerializerOptions? options = null) : this(options)
    {
        switch (defaults) {
            case SchemaDefaults.Default:
                break;
            case SchemaDefaults.OpenAi:
                // based on: https://platform.openai.com/docs/guides/structured-outputs/supported-schemas
                SchemaRootMode = SchemaRootMode.RootRecursion;
                FormatSupported = false;
                PropertyRequired = (property, context) => true;
                Validator = new OpenAiSchemaValidator();
                break;
        }
    }
}

public enum SchemaDefaults {
    Default = 0,
    OpenAi = 1,
}

public enum SchemaRootMode {
    RootRef = 0,
    RootRecursion = 1,
    RootDuplication = 3,
}

public delegate bool PropertyRequired(JsonPropertyType property, SchemaBuildContext context);
