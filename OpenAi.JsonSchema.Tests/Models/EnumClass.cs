namespace OpenAi.JsonSchema.Tests.Models;

public record EnumClass(
    int Id,
    Status Status
);

public enum Status {
    Default,
    PascalCase,
    CamelCase,
    SnakeCase,
    KebabCase,
}
