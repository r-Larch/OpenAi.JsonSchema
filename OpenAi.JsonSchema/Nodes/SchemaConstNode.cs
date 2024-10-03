using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaConstNode(object Value) : SchemaNode {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);
}
