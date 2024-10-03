using System.Diagnostics.CodeAnalysis;
using OpenAi.JsonSchema.Internals;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Generator;

public class DefinitionCollection {
    private readonly Dictionary<Type, SchemaRefValue> _values = new();
    private readonly DefinitionNameProvider _names = new();

    public IReadOnlyCollection<SchemaRefValue> Values => _values.Values;

    public virtual SchemaRefValue CreateRef(Type type, SchemaValueNode schema)
    {
        var value = new SchemaRefValue(schema, _names.GetName(type), Count: 1);
        _values.Add(type, value);
        return value;
    }

    public virtual bool TryGetRef(Type key, [NotNullWhen(true)] out SchemaRefValue? schema)
    {
        if (_values.TryGetValue(key, out schema)) {
            schema.Count++;
            return true;
        }

        return false;
    }
}
