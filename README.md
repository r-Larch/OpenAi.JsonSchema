# OpenAi JsonSchema

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/r-Larch/OpenAi.JsonSchema/ci.yml) ![NuGet Version](https://img.shields.io/nuget/v/LarchSys.OpenAi.JsonSchema)

**OpenAi-JsonSchema** is a lightweight library for generating valid JSON Schema for OpenAI's Structured Outputs feature, ensuring compatibility with OpenAI's JSON Schema subset. It simplifies the creation of structured outputs for OpenAI models, following the schema generation guidelines provided by OpenAI.

## Features
- Supports `System.ComponentModel.DescriptionAttribute` for generating descriptions in the JSON Schema.
- Handles `Nullable<T>` types (e.g., `int?`) and nullable reference types (e.g., `string?`).
- Supports a wide range of types, including primitives such as `bool`, `int`, `double`, and `DateTime`.
- Automatically manages `$defs` and `$ref` in the schema (e.g., `"$ref": "#/$defs/MyType"`).
- Ensures compatibility with OpenAI's JSON Schema format for structured outputs.

## Installation

Install via [NuGet](https://www.nuget.org/packages/LarchSys.OpenAi.JsonSchema):

```bash
Install-Package LarchSys.OpenAi.JsonSchema
```

## Quick Start

The following example demonstrates how to generate a JSON Schema using the **LarchSys.OpenAi.JsonSchema** library.

```csharp
// use Json Options to control PropertyName and Enum serialization:
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) {
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
};

// use SchemaDefaults.OpenAi to enforce OpenAi rule set:
var options = new JsonSchemaOptions(SchemaDefaults.OpenAi, jsonOptions);

var resolver = new DefaultSchemaGenerator();
var schema = resolver.Generate(type, options);

var json = schema.ToJsonNode().ToJsonString(new JsonSerializerOptions() { WriteIndented = true });
output.WriteLine(json);
Assert.NotNull(json);

[Description("A document")]
public record Document(
    [property: Description("Id of the document")] int Id,
    [property: Description("Document name")] string Name,
    [property: Description("Text lines of the document")] Line[] Lines,
    [property: Description("Next document in order")] Document? Next,
    [property: Description("Prev document in order")] Document? Prev
);

[Description("A line of text in a document")]
public record Line(
    [property: Description("Line number")] int Number,
    [property: Description("Line text")] string Text
);
```

### Example Output

```json
{
  "type": "object",
  "description": "A document",
  "properties": {
    "id": {
      "type": "integer",
      "description": "Id of the document"
    },
    "name": {
      "type": "string",
      "description": "Document name"
    },
    "lines": {
      "type": "array",
      "description": "Text lines of the document",
      "items": {
        "type": "object",
        "description": "A line of text in a document",
        "properties": {
          "number": {
            "type": "integer",
            "description": "Line number"
          },
          "text": {
            "type": "string",
            "description": "Line text"
          }
        },
        "required": [
          "number",
          "text"
        ],
        "additionalProperties": false
      }
    },
    "next": {
      "description": "Next document in order",
      "anyOf": [
        { "type": "null" },
        { "$ref": "#" }
      ]
    },
    "prev": {
      "description": "Prev document in order",
      "anyOf": [
        { "type": "null" },
        { "$ref": "#" }
      ]
    }
  },
  "required": [
    "id",
    "name",
    "lines",
    "next",
    "prev"
  ],
  "additionalProperties": false
}

```

## How It Works

OpenAi JsonSchema simplifies the generation of JSON Schema for structured outputs using C# classes and attributes. It leverages the `System.ComponentModel.DescriptionAttribute` for field descriptions and supports nullable reference types, ensuring full compatibility with the JSON Schema language supported by OpenAI models.

For more details on the OpenAI Structured Outputs feature, check out:
- [Introducing Structured Outputs in the OpenAI API](https://openai.com/index/introducing-structured-outputs-in-the-api/)
- [OpenAI Structured Outputs Guide](https://platform.openai.com/docs/guides/structured-outputs/introduction)

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request with any improvements or feature additions. All contributions should follow the repository's guidelines.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for more information.

## Contact

For any questions or issues, feel free to reach out via GitHub or email.
