using LargeLanguageModelClient.Dto.Model;

namespace Interface.Service;

public interface IModelService
{
    Task<LlmModelDto?> GetModel(Guid modelIdentifier);

    Task<List<LlmModelDto>?> GetAllModels();
}
