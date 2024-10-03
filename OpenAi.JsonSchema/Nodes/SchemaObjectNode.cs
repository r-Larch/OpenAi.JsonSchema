using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaObjectNode(
    Dictionary<string, SchemaNode> Properties,
    List<string> Required,
    bool? AdditionalProperties = null
) : SchemaValueNode("object", Nullable: false) {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);

    public void AddProperty(PropertySchema property)
    {
        var (name, propertySchema, required) = property;

        Properties[name] = propertySchema;

        if (required) {
            Required.Add(name);
        }
    }
}

public record struct PropertySchema(
    string Name,
    SchemaNode Schema,
    bool Required
);
