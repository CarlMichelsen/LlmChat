using Domain.Dto;

namespace Interface.Handler;

public interface IProfileHandler
{
    Task<ServiceResponse<string>> SetDefaultSystemMessage(string systemMessage);

    Task<ServiceResponse<string>> GetDefaultSystemMessage();
}
