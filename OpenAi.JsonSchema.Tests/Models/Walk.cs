using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Tests.Models;

public static class Schema {
    public static IReadOnlyCollection<T> All<T>(this SchemaNode schema) where T : SchemaNode
    {
        var visitor = new WalkVisitor<T>();
        visitor.Visit(schema);
        return visitor.List;
    }

    public static T Single<T>(this SchemaNode schema) where T : SchemaNode
    {
        return schema.All<T>().Single();
    }

    private class WalkVisitor<T> : SchemaVisitor {
        public List<T> List { get; } = [];

        public override void Visit(SchemaNode schema)
        {
            if (schema is T x) {
                List.Add(x);
            }

            base.Visit(schema);
        }
    }
}
