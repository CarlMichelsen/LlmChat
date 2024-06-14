using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class DialogSliceDto
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("messages")]
    public required List<MessageDto> Messages { get; set; }

    [JsonPropertyName("selectedIndex")]
    public required int SelectedIndex { get; set; }

    [JsonPropertyName("visible")]
    public required bool Visible { get; set; }
}
