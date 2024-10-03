using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaAnyOfNode(params SchemaNode[] Options) : SchemaNode {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);
}
