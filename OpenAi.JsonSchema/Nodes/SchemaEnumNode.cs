using OpenAi.JsonSchema.Nodes.Abstractions;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaEnumNode(string Type, string[] Values, bool Nullable) : SchemaValueNode(Type, Nullable) {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);


    public static SchemaEnumNode Create(Type @enum, bool nullable, JsonSerializerOptions options)
    {
        var values = Enum.GetValues(@enum).Cast<object>().ToArray();

        var jsonValues = values.Select(value => JsonSerializer.Serialize(value, options)).ToArray();

        var kind = jsonValues.Select(_ => JsonValue.Parse(_)!.GetValueKind()).FirstOrDefault();
        var type = kind switch {
            JsonValueKind.String => "string",
            JsonValueKind.Number => "integer",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };

        return new SchemaEnumNode(type, jsonValues, nullable);
    }
}
