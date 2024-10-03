using System.Diagnostics;
using System.Text.Json.Serialization.Metadata;


namespace OpenAi.JsonSchema.Generator.Abstractions;

[DebuggerDisplay("{ToString(),nq}")]
public class JsonType {
    private readonly JsonTypeInfo _info;
    private readonly JsonTypeResolver _resolver;
    private readonly JsonNullableInfo? _nullableInfo;
    private JsonType[]? _genericTypeArguments;
    private IList<JsonPropertyType>? _properties;
    private JsonType[]? _polymorphismOptions;
    private string? _typeDiscriminatorName;
    private object? _typeDiscriminatorValue;

    internal JsonType(JsonTypeResolver resolver, JsonTypeInfo info, JsonNullableInfo? nullable, object? value)
    {
        _info = info;
        _resolver = resolver;
        _nullableInfo = nullable;

        Kind = info.Kind switch {
            JsonTypeInfoKind.Object => JsonTypeKind.Object,
            JsonTypeInfoKind.Dictionary => JsonTypeKind.Object,
            JsonTypeInfoKind.Enumerable => JsonTypeKind.Array,
            JsonTypeInfoKind.None => JsonTypeKind.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(info.Kind), info.Kind, null)
        };

        Type = System.Nullable.GetUnderlyingType(info.Type) ?? info.Type;

        Value = value;
    }

    public JsonTypeKind Kind { get; }
    public Type Type { get; }
    public bool? Nullable => _nullableInfo?.Nullable;
    public object? Value { get; }

    public JsonType? ElementType {
        get {
            var e = Type.GetElementType();
            return e is null ? null : _resolver.GetTypeInfo(e, _nullableInfo?.ElementType);
        }
    }

    public JsonType[] GenericTypeArguments {
        get { return _genericTypeArguments ??= Type.GenericTypeArguments.Select((e, i) => _resolver.GetTypeInfo(e, _nullableInfo?.GetGenericTypeArgument(i))).ToArray(); }
    }

    public IList<JsonPropertyType> Properties {
        get { return _properties ??= _info.Properties.Select(_ => new JsonPropertyType(_resolver, this, _)).ToList(); }
    }

    public JsonType[]? PolymorphismOptions => _info.PolymorphismOptions is null ? _polymorphismOptions : _polymorphismOptions ??= CreatePolymorphismOptions().ToArray();

    public string? TypeDiscriminatorName {
        get => _typeDiscriminatorName ??= _info.PolymorphismOptions?.TypeDiscriminatorPropertyName;
        private set => _typeDiscriminatorName = value;
    }
    public object? TypeDiscriminatorValue {
        get => _typeDiscriminatorValue ??= _info.PolymorphismOptions?.DerivedTypes.FirstOrDefault(_ => _.DerivedType == Type).TypeDiscriminator;
        private set => _typeDiscriminatorValue = value;
    }

    private IEnumerable<JsonType> CreatePolymorphismOptions()
    {
        if (_info.PolymorphismOptions is null) yield break;
        foreach (var derivedType in _info.PolymorphismOptions.DerivedTypes) {
            var type = _resolver.GetTypeInfo(derivedType.DerivedType, _nullableInfo);
            type.TypeDiscriminatorName = TypeDiscriminatorName;
            type.TypeDiscriminatorValue = derivedType.TypeDiscriminator;
            yield return type;
        }
    }

    public JsonPropertyType CreateProperty(Type propertyType, string name, object? value = null)
    {
        var property = _info.CreateJsonPropertyInfo(propertyType, name);
        return new JsonPropertyType(_resolver, this, property, value);
    }

    public override string ToString()
    {
        var nulls = Nullable is true ? "?" : string.Empty;
        var gens = GenericTypeArguments.Length > 0 ? $"<{string.Join(", ", GenericTypeArguments.Select(_ => _.ToString()))}>" : string.Empty;
        var type = Type.Name + nulls + gens;

        if (Value is not null) {
            type = $"Const({type} {Value})";
        }

        return type;
    }
}

public enum JsonTypeKind {
    Object,
    Array,
    Value,
}
