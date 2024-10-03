namespace OpenAi.JsonSchema.Nodes.Abstractions;

public abstract class SchemaTransformer<T> {
    public virtual T Transform(SchemaNode schema) => schema.Accept(this);
    public virtual T Transform(SchemaRootNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaRefNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaConstNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaValueNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaFormatNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaEnumNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaObjectNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaArrayNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaDefinitionNode schema) => throw new NotImplementedException();
    public virtual T Transform(SchemaAnyOfNode schema) => throw new NotImplementedException();
}
