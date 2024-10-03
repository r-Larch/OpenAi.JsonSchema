using System.Text.Json.Serialization;


namespace OpenAi.JsonSchema.Tests.Models;

public class Parent {
    public required int Age { get; set; }
    public string Name { get; set; } = null!;
    public Child[]? Children { get; set; }
}

[JsonDerivedType(typeof(Child), "child")]
[JsonDerivedType(typeof(StepChild), "step-child")]
public record Child(
    string Name,
    int Age,
    DateTimeOffset BirthDate,
    Parent? Parent,
    Child[] Siblings
);

public record StepChild(
    string Name,
    int Age,
    DateTimeOffset BirthDate,
    Parent? Parent,
    Child[] Siblings,
    Child[] StepSiblings
) : Child(Name, Age, BirthDate, Parent, Siblings);
