using System.ComponentModel;
using System.Reflection;
using OpenAi.JsonSchema.Generator.Abstractions;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Generator;

public class DefaultObjectSchemaBuilder() : IObjectSchemaBuilder {
    public virtual SchemaNode BuildSchema(JsonType type, SchemaBuildContext context)
    {
        if (type.PolymorphismOptions is { } options) {
            var schemas = options.Select(type => BuildObjectSchemaCached(type, context)).ToArray();
            return new SchemaAnyOfNode(schemas);
        }

        return BuildObjectSchemaCached(type, context);
    }

    protected virtual SchemaNode BuildObjectSchemaCached(JsonType type, SchemaBuildContext context)
    {
        if (!context.Definitions.TryGetRef(type.Type, out var @ref)) {
            @ref = BuildObjectSchema(type, context);
        }

        var refNode = new SchemaRefNode(@ref);

        if (type.Nullable is true) {
            return new SchemaAnyOfNode(new SchemaValueNode("null"), refNode);
        }

        return refNode;
    }

    protected virtual SchemaRefValue BuildObjectSchema(JsonType type, SchemaBuildContext context)
    {
        var schema = new SchemaObjectNode([], [], false);

        var @ref = context.Definitions.CreateRef(type.Type, schema);

        if (type.Type.GetCustomAttribute(typeof(DescriptionAttribute), inherit: false) is DescriptionAttribute description) {
            schema.Description = description.Description;
        }

        if (type is { TypeDiscriminatorName: { } name, TypeDiscriminatorValue: { } value }) {
            var property = type.CreateProperty(value.GetType(), name, value);
            var propertySchema = BuildPropertySchema(property, context);
            schema.AddProperty(propertySchema);
        }

        foreach (var property in type.Properties) {
            var propertySchema = BuildPropertySchema(property, context);
            schema.AddProperty(propertySchema);
        }

        return @ref;
    }

    protected virtual PropertySchema BuildPropertySchema(JsonPropertyType property, SchemaBuildContext context)
    {
        var schema = context.Generate(property.PropertyType);

        var required = (context.Options.PropertyRequired ?? PropertyRequired).Invoke(property, context);

        var descriptionAttribute = property.Attributes.GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().FirstOrDefault();
        if (descriptionAttribute is not null) {
            schema.Description = descriptionAttribute.Description;
        }

        return new PropertySchema(
            Name: property.Name,
            Schema: schema,
            Required: required
        );
    }

    protected virtual bool PropertyRequired(JsonPropertyType property, SchemaBuildContext context)
    {
        var nullable = property.PropertyType.Nullable is true;
        return property.IsRequired || !nullable;
    }
}
