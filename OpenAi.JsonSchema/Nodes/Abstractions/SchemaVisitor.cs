namespace OpenAi.JsonSchema.Nodes.Abstractions;

public abstract class SchemaVisitor {
    public virtual void Visit(SchemaNode schema) => schema.Accept(this);

    public virtual void Visit(SchemaRootNode schema)
    {
        Visit(schema.Root);
        Visit((SchemaNode) schema.Definitions);
    }

    public virtual void Visit(SchemaRefNode schema)
    {
        //Visit(schema.Ref);
    }

    public virtual void Visit(SchemaConstNode schema)
    {
    }

    public virtual void Visit(SchemaValueNode schema)
    {
    }

    public virtual void Visit(SchemaFormatNode schema)
    {
        Visit((SchemaValueNode) schema);
    }

    public virtual void Visit(SchemaEnumNode schema)
    {
        Visit((SchemaValueNode) schema);
    }

    public virtual void Visit(SchemaObjectNode schema)
    {
        foreach (var (_, value) in schema.Properties) {
            Visit(value);
        }

        Visit((SchemaValueNode) schema);
    }

    public virtual void Visit(SchemaArrayNode schema)
    {
        Visit(schema.Items);

        Visit((SchemaValueNode) schema);
    }

    public virtual void Visit(SchemaDefinitionNode definitions)
    {
        foreach (var schema in definitions.Values) {
            Visit(schema.Value);
        }
    }

    public virtual void Visit(SchemaAnyOfNode schema)
    {
        foreach (var node in schema.Options) {
            Visit(node);
        }
    }
}
