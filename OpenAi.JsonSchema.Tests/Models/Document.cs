using System.ComponentModel;


namespace OpenAi.JsonSchema.Tests.Models;

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
