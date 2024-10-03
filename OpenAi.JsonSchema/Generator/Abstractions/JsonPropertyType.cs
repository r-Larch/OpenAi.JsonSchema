using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;


namespace OpenAi.JsonSchema.Generator.Abstractions;

[DebuggerDisplay("{ToString(),nq}")]
public class JsonPropertyType {
    private static readonly PropertyInfo MemberNameProperty = typeof(JsonPropertyInfo).GetProperty("MemberName", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new Exception("Failed to access JsonTypeInfo.MemberName using reflection!");

    private readonly JsonNullableInfo? _nullableInfo;
    private readonly JsonTypeResolver _resolver;
    private readonly JsonPropertyInfo _property;
    private readonly object? _value;
    private JsonType? _propertyType;

    internal JsonPropertyType(JsonTypeResolver resolver, JsonType declaringType, JsonPropertyInfo property, object? value = null)
    {
        _resolver = resolver;
        _property = property;
        _value = value;
        DeclaringType = declaringType;
        _nullableInfo = resolver.GetNullabilityInfo(this);
        _propertyType = null;
    }

    public JsonType DeclaringType { get; }
    public JsonType PropertyType => _propertyType ??= _resolver.GetTypeInfo(_property.PropertyType, _nullableInfo, _value);

    public string Name => _property.Name;
    public string MemberName => (string?) MemberNameProperty.GetValue(_property) ?? Name;

    public bool IsRequired => _property.IsRequired;
    public ICustomAttributeProvider Attributes => _property.AttributeProvider ?? new NoopAttributeProvider();

    public override string ToString()
    {
        return $$"""[{{DeclaringType}}] {{PropertyType}} {{MemberName}} { get; set; }""";
    }
}

internal readonly struct NoopAttributeProvider : ICustomAttributeProvider {
    public object[] GetCustomAttributes(bool inherit) => [];
    public object[] GetCustomAttributes(Type attributeType, bool inherit) => [];
    public bool IsDefined(Type attributeType, bool inherit) => false;
}
