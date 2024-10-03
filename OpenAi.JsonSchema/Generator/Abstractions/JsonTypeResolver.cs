using System.Reflection;
using System.Text.Json;


namespace OpenAi.JsonSchema.Generator.Abstractions;

public class JsonTypeResolver {
    private readonly JsonSerializerOptions _options;
    private readonly NullabilityInfoContext _context = new();

    public JsonTypeResolver(JsonSerializerOptions options)
    {
        if (!options.IsReadOnly) options.MakeReadOnly(populateMissingResolver: true);
        _options = options;
    }

    public JsonType GetTypeInfo(Type type)
    {
        return GetTypeInfo(type, nullable: null);
    }

    internal JsonType GetTypeInfo(Type type, JsonNullableInfo? nullable, object? value = null)
    {
        var typeInfo = _options.TypeInfoResolver?.GetTypeInfo(type, _options) ?? throw new Exception($"Failed to Resolve JsonTypeInfo for type: {type.FullName}");
        return new JsonType(this, typeInfo, nullable, value);
    }

    internal JsonNullableInfo? GetNullabilityInfo(JsonPropertyType property)
    {
        var info = GetNullabilityInfo(property.DeclaringType.Type, property.MemberName);
        return info is null ? null : new JsonNullableInfo(info);
    }

    private NullabilityInfo? GetNullabilityInfo(Type type, string memberName)
    {
        var member = type.GetMember(memberName).FirstOrDefault();

        var nullability = member switch {
            PropertyInfo x => _context.Create(x),
            FieldInfo x => _context.Create(x),
            EventInfo x => _context.Create(x),
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(member), member?.GetType().FullName, null)
        };

        return nullability;
    }
}

internal readonly struct JsonNullableInfo(NullabilityInfo info) {
    public JsonNullableInfo? ElementType => info.ElementType is { } x ? new(x) : null;
    public JsonNullableInfo GetGenericTypeArgument(int i) => new(info.GenericTypeArguments[i]);

    public NullabilityState ReadState => info.ReadState;
    public NullabilityState WriteState => info.WriteState;

    public bool Nullable => info.ReadState is NullabilityState.Nullable || info.WriteState is NullabilityState.Nullable;
}
