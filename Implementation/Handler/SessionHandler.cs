using Domain.Dto;
using Domain.Session;
using Interface.Handler;
using Interface.Service;

namespace Implementation.Handler;

public class SessionHandler(
    ISessionService sessionService) : ISessionHandler
{
    public async Task<ServiceResponse<UserData>> GetSessionData()
    {
        var sessionData = await sessionService.GetSessionData();
        if (sessionData.IsError)
        {
            var notLoggedInRes = new ServiceResponse<UserData>("User not logged in");
            return notLoggedInRes;
        }

        var loggedInRes = new ServiceResponse<UserData>(sessionData.Unwrap().User);
        return loggedInRes;
    }

    public async Task<ServiceResponse<bool>> RemoveSessionData()
    {
        var sessionData = await sessionService.RemoveSessionData();
        if (sessionData.IsError)
        {
            return new ServiceResponse<bool>("Failed to logout user");
        }

        return new ServiceResponse<bool>(true);
    }
}
