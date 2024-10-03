using System.Diagnostics.CodeAnalysis;
using OpenAi.JsonSchema.Nodes;


namespace OpenAi.JsonSchema.Validation;

public abstract class SchemaValidator {
    public abstract ValidationResult Validate(SchemaRootNode schema);
}

public class ValidationResult(IReadOnlyCollection<string>? errors = null) {
    [MemberNotNullWhen(false, nameof(Errors))]
    public bool Valid { get; init; } = errors is not { Count: > 0 };

    public IReadOnlyCollection<string>? Errors { get; init; } = errors;
}
