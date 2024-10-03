using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Nodes;

public record SchemaRefNode(SchemaRefValue Ref) : SchemaNode {
    internal override void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal override T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);
}

public record SchemaRefValue(SchemaNode Value, string Name, int Count) {
    internal int Count { get; set; } = Count;
    internal bool Root { get; set; }
}
