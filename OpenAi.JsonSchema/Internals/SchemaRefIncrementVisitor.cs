using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Internals;

internal class SchemaRefIncrementVisitor : SchemaVisitor {
    private readonly HashSet<SchemaRefValue> _seen = [];

    public override void Visit(SchemaRootNode schema)
    {
        Visit(schema.Root);
    }

    public override void Visit(SchemaRefNode schema)
    {
        if (_seen.Add(schema.Ref)) {
            schema.Ref.Count++;
        }
    }
}
