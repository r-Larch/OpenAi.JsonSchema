namespace OpenAi.JsonSchema.Internals;

internal readonly struct DefinitionNameProvider() {
    private readonly Dictionary<string, int> _counts = new();

    public string GetName(Type type)
    {
        var name = type.Name;
        var count = _counts.GetValueOrDefault(name, 0) + 1;
        _counts[name] = count;

        return count == 1 ? name : $"{name}{count}";
    }
}
