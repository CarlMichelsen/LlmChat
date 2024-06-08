using Domain.Dto;
using Domain.Dto.LargeLanguageModel;

namespace Interface.Handler;

public interface IModelHandler
{
    Task<ServiceResponse<List<LargeLanguageModelDto>>> GetAvailableModels();
}
