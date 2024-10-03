using System.Text.Json;
using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Serialization;


namespace OpenAi.JsonSchema.Generator.Abstractions;

public interface ISchemaGenerator {
    public SchemaRootNode Generate(Type type, JsonSerializerOptions? options = null);
    public SchemaRootNode Generate(Type type, JsonSchemaOptions? options = null);
    public SchemaRootNode Generate<T>(JsonSerializerOptions? options = null) => Generate(typeof(T), options);
    public SchemaRootNode Generate<T>(JsonSchemaOptions? options = null) => Generate(typeof(T), options);
}
