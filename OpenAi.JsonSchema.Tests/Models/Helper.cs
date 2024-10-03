using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using OpenAi.JsonSchema.Generator;
using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Serialization;


namespace OpenAi.JsonSchema.Tests.Models;

public static class Helper {
    public static readonly JsonSerializerOptions JsonOptions = new() {
        WriteIndented = true,
    };

    public static readonly JsonSerializerOptions JsonOptionsCamelCase = new(JsonSerializerDefaults.Web) {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static readonly JsonSerializerOptions JsonOptionsSnakeCase = new(JsonSerializerDefaults.Web) {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };

    public static SchemaRootNode Generate<T>(JsonSchemaOptions? options = null)
    {
        return Generate(typeof(T), options);
    }

    public static SchemaRootNode Generate(Type type, JsonSchemaOptions? options = null)
    {
        options ??= new JsonSchemaOptions(JsonOptions);

        var resolver = new DefaultSchemaGenerator();
        var schema = resolver.Generate(type, options);

        return schema;
    }

    public static JsonNode GenerateJsonNode(Type type)
    {
        return Generate(type).ToJsonNode();
    }
}