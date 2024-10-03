using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Validation;

public class OpenAiSchemaValidator : SchemaValidator {
    public override ValidationResult Validate(SchemaRootNode schema)
    {
        var visitor = new OpenAiSchemaVisitor();

        visitor.Visit(schema);

        return new ValidationResult(visitor.Errors);
    }

    private class OpenAiSchemaVisitor : SchemaVisitor {
        public readonly List<string> Errors = [];
        private static readonly HashSet<string> SupportedTypes = [
            "string",
            "number",
            "boolean",
            "integer",
            "object",
            "array",
            "enum",
            "null",
        ];

        public override void Visit(SchemaRootNode schema)
        {
            if (schema.Root is SchemaAnyOfNode) {
                Errors.Add($"Unsupported 'anyOf' at root");
            }

            base.Visit(schema);
        }

        public override void Visit(SchemaValueNode schema)
        {
            if (!SupportedTypes.Contains(schema.Type)) {
                Errors.Add($"Unsupported Type: '{schema.Type}' SupportedTypes: {string.Join(", ", SupportedTypes)}");
            }

            base.Visit(schema);
        }

        public override void Visit(SchemaFormatNode schema)
        {
            Errors.Add($"Format Node unsupported: {schema.Type} format: {schema.Format}");
            base.Visit(schema);
        }

        public override void Visit(SchemaObjectNode schema)
        {
            //var hasDiscriminator = schema is { TypeDiscriminatorName: not null, TypeDiscriminatorValue: not null };
            var properties = schema.Properties.Count; //+ (hasDiscriminator ? 1 : 0);

            if (properties != schema.Required.Count) {
                Errors.Add($"All properties must be required!");
            }

            if (schema.AdditionalProperties is not false) {
                Errors.Add($"AdditionalProperties must be false!");
            }

            base.Visit(schema);
        }
    }
}
