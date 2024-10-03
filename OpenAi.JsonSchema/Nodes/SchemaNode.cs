using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Nodes;

public abstract record SchemaNode() {
    public string? Description { get; set; }

    internal virtual void Accept(SchemaVisitor visitor) => visitor.Visit(this);
    internal virtual T Accept<T>(SchemaTransformer<T> visitor) => visitor.Transform(this);
}
