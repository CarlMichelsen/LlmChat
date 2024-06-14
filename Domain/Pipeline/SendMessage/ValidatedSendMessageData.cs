using Domain.Entity;
using LargeLanguageModelClient.Dto.Model;

namespace Domain.Pipeline.SendMessage;

public class ValidatedSendMessageData
{
    public required ResponseToData? ResponseTo { get; init; }

    public required LlmModelDto SelectedModel { get; init; }

    public required List<ContentEntity> Content { get; init; }
}
