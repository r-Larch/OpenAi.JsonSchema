using OpenAi.JsonSchema.Generator.Abstractions;
using OpenAi.JsonSchema.Nodes;
using System.ComponentModel;
using System.Reflection;


namespace OpenAi.JsonSchema.Generator;

public class DefaultArraySchemaBuilder() : IArraySchemaBuilder {
    public virtual SchemaNode BuildSchema(JsonType type, SchemaBuildContext context)
    {
        var elementType = type.ElementType ?? type.GenericTypeArguments[0];

        var schema = new SchemaArrayNode(
            Items: context.Generate(elementType)
        );

        if (type.Type.GetCustomAttribute(typeof(DescriptionAttribute), inherit: false) is DescriptionAttribute description) {
            schema.Description = description.Description;
        }

        if (type.Nullable is true) {
            return new SchemaAnyOfNode(new SchemaValueNode("null"), schema);
        }

        return schema;
    }
}
