using Domain.Dto;

namespace Interface.Handler;

public interface IProfileHandler
{
    Task<ServiceResponse<string>> SetDefaultSystemMessage(string systemMessage);
}
