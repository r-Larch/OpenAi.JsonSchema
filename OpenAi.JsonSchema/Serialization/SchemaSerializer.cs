using System.Text.Json;
using System.Text.Json.Nodes;
using OpenAi.JsonSchema.Internals;
using OpenAi.JsonSchema.Nodes;
using OpenAi.JsonSchema.Nodes.Abstractions;


namespace OpenAi.JsonSchema.Serialization;

internal class SchemaSerializer(JsonSchemaOptions options) : SchemaTransformer<JsonNode> {
    //private int Level { get; set; }
    //public override JsonNode Transform(SchemaNode schema)
    //{
    //    Level++;
    //    var node = base.Transform(schema);
    //    node.AsObject()["level"] = JsonValue.Create(Level);
    //    Level--;
    //    return node;
    //}

    public override JsonNode Transform(SchemaRootNode schema)
    {
        var root = schema.Root;

        if (root is SchemaRefNode { Ref: { } @ref }) {
            if (options.SchemaRootMode is SchemaRootMode.RootDuplication) {
                @ref.Count--;
                root = @ref.Value;
                new SchemaRefIncrementVisitor().Visit(@ref.Value);
            }
            else if (options.SchemaRootMode is SchemaRootMode.RootRecursion) {
                @ref.Root = true;
                root = @ref.Value;
            }
        }

        var node = Transform(root);
        var definitions = Transform(schema.Definitions);

        AddDescription(node, root.Description);

        if (definitions.Count > 0) {
            node["$defs"] = definitions;
        }

        return node;
    }


    public override JsonObject Transform(SchemaDefinitionNode schema)
    {
        var node = new JsonObject();
        foreach (var value in schema.Values) {
            if (value is { Count: > 1, Root: false }) {
                node.Add(value.Name, Transform(value.Value));
            }
        }

        return node;
    }

    public override JsonNode Transform(SchemaConstNode schema)
    {
        var value = JsonSerializer.SerializeToNode(schema.Value, options.JsonSerializerOptions);

        var node = new JsonObject {
            ["const"] = value
        };

        AddDescription(node, schema.Description);

        return node;
    }

    public override JsonNode Transform(SchemaValueNode schema)
    {
        var node = new JsonObject {
            ["type"] = schema.Nullable ? new JsonArray([schema.Type, "null"]) : schema.Type,
        };

        AddDescription(node, schema.Description);

        return node;
    }

    public override JsonNode Transform(SchemaFormatNode schema)
    {
        var node = new JsonObject {
            ["type"] = schema.Nullable ? new JsonArray([schema.Type, "null"]) : schema.Type,
            ["format"] = JsonValue.Create(schema.Format)
        };

        AddDescription(node, schema.Description);

        return node;
    }

    public override JsonNode Transform(SchemaEnumNode schema)
    {
        var node = new JsonObject {
            ["type"] = schema.Nullable ? new JsonArray(schema.Type, "null") : schema.Type
        };

        AddDescription(node, schema.Description);

        node["enum"] = new JsonArray(schema.Values.Select(_ => JsonNode.Parse(_)).ToArray());

        return node;
    }

    public override JsonNode Transform(SchemaArrayNode schema)
    {
        var node = new JsonObject {
            ["type"] = schema.Type
        };

        AddDescription(node, schema.Description);

        node["items"] = Transform(schema.Items);

        return node;
    }

    public override JsonNode Transform(SchemaObjectNode schema)
    {
        var properties = schema.Properties
            .ToDictionary(
                string (_) => _.Key,
                JsonNode? (_) => Transform(_.Value)
            )
            .ToList();

        var node = new JsonObject {
            ["type"] = schema.Type
        };

        AddDescription(node, schema.Description);

        node["properties"] = new JsonObject(properties);

        if (schema.Required.Count > 0) {
            node["required"] = new JsonArray(schema.Required.Select(JsonNode (_) => JsonValue.Create(_)).ToArray());
        }

        if (schema.AdditionalProperties is not null) {
            node["additionalProperties"] = schema.AdditionalProperties;
        }

        return node;
    }

    public override JsonNode Transform(SchemaRefNode schema)
    {
        if (schema.Ref.Count <= 1) {
            schema.Ref.Count--;
            return Transform(schema.Ref.Value);
        }

        var @ref = schema.Ref.Root
            ? "#" // root recursion: "$ref": "#"
            : $"#/$defs/{schema.Ref.Name}";

        return new JsonObject {
            ["$ref"] = @ref,
        };
    }

    public override JsonNode Transform(SchemaAnyOfNode schema)
    {
        var items = schema.Options.Select(Transform).ToArray();

        var node = new JsonObject();

        AddDescription(node, schema.Description);

        node["anyOf"] = new JsonArray(items);

        return node;
    }


    private static void AddDescription(JsonNode node, string? description)
    {
        if (description is not null) {
            node["description"] = JsonValue.Create(description);
        }
    }
}
