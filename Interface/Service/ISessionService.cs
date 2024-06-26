﻿using Domain.Abstraction;
using Domain.Session;

namespace Interface.Service;

public interface ISessionService
{
    Guid UserProfileId { get; }
    
    Task<Result<SessionData>> GetSessionData();

    Task<Result<bool>> RemoveSessionData();
}