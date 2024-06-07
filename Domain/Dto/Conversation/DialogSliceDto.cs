using System.Text.Json.Serialization;

namespace Domain.Dto.Conversation;

public class DialogSliceDto
{
    [JsonPropertyName("messages")]
    public required List<MessageDto> Messages { get; set; }

    [JsonPropertyName("selectedIndex")]
    public required int SelectedIndex { get; init; }

    [JsonPropertyName("visible")]
    public required bool Visible { get; set; }
}
