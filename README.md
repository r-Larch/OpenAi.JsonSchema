# OpenAi-JsonSchema

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/r-Larch/OpenAi.JsonSchema/ci.yml) ![NuGet Version](https://img.shields.io/nuget/v/OpenAi-JsonSchema)

**OpenAi-JsonSchema** is a lightweight library for generating valid JSON Schema for OpenAI's Structured Outputs feature, ensuring compatibility with OpenAI's JSON Schema subset. It simplifies the creation of structured outputs for OpenAI models, following the schema generation guidelines provided by OpenAI.

## Features
- Supports `System.ComponentModel.DescriptionAttribute` for descriptions.
- Handles nullable reference types.
- Supports a wide range of types, including primitives (e.g., `bool`, `int`, `double`, `DateTime`).
- Ensures compatibility with OpenAI's JSON Schema format.

## Installation

Install via [NuGet](https://www.nuget.org/packages/OpenAi-JsonSchema):

```bash
Install-Package OpenAi-JsonSchema
```

## Quick Start

The following example demonstrates how to generate a JSON Schema using the **OpenAi-JsonSchema** library.

```csharp
var options = new JsonSchemaOptions(SchemaDefaults.OpenAi);

var resolver = new DefaultSchemaGenerator();
var schema = resolver.Generate<Document>(options);

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
    "Id": {
      "type": "integer",
      "description": "Id of the document"
    },
    "Name": {
      "type": "string",
      "description": "Document name"
    },
    "Lines": {
      "type": "array",
      "description": "Text lines of the document",
      "items": {
        "type": "object",
        "description": "A line of text in a document",
        "properties": {
          "Number": {
            "type": "integer",
            "description": "Line number"
          },
          "Text": {
            "type": "string",
            "description": "Line text"
          }
        },
        "required": [
          "Number",
          "Text"
        ],
        "additionalProperties": false
      }
    },
    "Next": {
      "description": "Next document in order",
      "anyOf": [
        { "type": "null" },
        { "$ref": "#" }
      ]
    },
    "Prev": {
      "description": "Prev document in order",
      "anyOf": [
        { "type": "null" },
        { "$ref": "#" }
      ]
    }
  },
  "required": [
    "Id",
    "Name",
    "Lines",
    "Next",
    "Prev"
  ],
  "additionalProperties": false
}
```

## How It Works

OpenAi-JsonSchema simplifies the generation of JSON Schema for structured outputs using C# classes and attributes. It leverages the `System.ComponentModel.DescriptionAttribute` for field descriptions and supports nullable reference types, ensuring full compatibility with the JSON Schema language supported by OpenAI models.

For more details on the OpenAI Structured Outputs feature, check out:
- [Introducing Structured Outputs in the OpenAI API](https://openai.com/index/introducing-structured-outputs-in-the-api/)
- [OpenAI Structured Outputs Guide](https://platform.openai.com/docs/guides/structured-outputs/introduction)

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request with any improvements or feature additions. All contributions should follow the repository's guidelines.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Contact

For any questions or issues, feel free to reach out via GitHub or email.
