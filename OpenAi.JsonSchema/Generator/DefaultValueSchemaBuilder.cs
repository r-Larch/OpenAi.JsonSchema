using OpenAi.JsonSchema.Generator.Abstractions;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Generator;

public class DefaultValueSchemaBuilder() : IValueSchemaBuilder {
    public virtual SchemaNode BuildSchema(JsonType info, SchemaBuildContext context)
    {
        var type = info.Type;
        var nullable = info.Nullable is true;

        if (info.Value is { } value) {
            return new SchemaConstNode(value);
        }
        else if (type == typeof(string) || type == typeof(char)) {
            return new SchemaValueNode("string", nullable);
        }
        else if (type == typeof(int) ||
                 type == typeof(long) ||
                 type == typeof(uint) ||
                 type == typeof(byte) ||
                 type == typeof(sbyte) ||
                 type == typeof(ulong) ||
                 type == typeof(short) ||
                 type == typeof(ushort)) {
            return new SchemaValueNode("integer", nullable);
        }
        else if (type == typeof(float) ||
                 type == typeof(double) ||
                 type == typeof(decimal)) {
            return new SchemaValueNode("number", nullable);
        }
        else if (type == typeof(bool)) {
            return new SchemaValueNode("boolean", nullable);
        }
        else if (type == typeof(DateTime) || type == typeof(DateTimeOffset)) {
            return context.Options.FormatSupported
                ? new SchemaFormatNode("string", "date-time", nullable)
                : new SchemaValueNode("string", nullable) { Description = "date-time" };
        }
        else if (type == typeof(DateOnly)) {
            return context.Options.FormatSupported
                ? new SchemaFormatNode("string", "date", nullable)
                : new SchemaValueNode("string", nullable) { Description = "date" };
        }
        else if (type == typeof(TimeOnly)) {
            return context.Options.FormatSupported
                ? new SchemaFormatNode("string", "time", nullable)
                : new SchemaValueNode("string", nullable) { Description = "time" };
        }
        else if (type == typeof(Guid)) {
            return context.Options.FormatSupported
                ? new SchemaFormatNode("string", "uuid", nullable)
                : new SchemaValueNode("string", nullable) { Description = "uuid" };
        }
        else if (type.IsEnum) {
            return SchemaEnumNode.Create(type, nullable, context.Options.JsonSerializerOptions);
        }
        else if (GetDefault(type) is IFormattable) {
            return new SchemaValueNode("string", nullable);
        }
        else {
            throw new ArgumentOutOfRangeException(nameof(type), type.FullName, null);
        }
    }

    private static object? GetDefault(Type type) => Array.CreateInstance(type, 1).GetValue(0);
}
