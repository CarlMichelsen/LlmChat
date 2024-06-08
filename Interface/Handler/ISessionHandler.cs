using Domain.Dto;
using Domain.Session;

namespace Interface.Handler;

public interface ISessionHandler
{
    Task<ServiceResponse<UserData>> GetSessionData();

    Task<ServiceResponse<bool>> RemoveSessionData();
}
