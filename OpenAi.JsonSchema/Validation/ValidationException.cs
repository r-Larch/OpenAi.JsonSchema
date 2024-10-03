namespace OpenAi.JsonSchema.Validation;

public class ValidationException(IReadOnlyCollection<string> errors) : Exception($"Schema Validation Errors:\n- {string.Join("\n- ", errors)}") {
    public IReadOnlyCollection<string> Errors { get; } = errors;
}
