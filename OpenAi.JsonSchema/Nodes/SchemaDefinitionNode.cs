using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaDefinitionNode(IReadOnlyCollection<SchemaRefValue> Values) : SchemaNode {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);
}
