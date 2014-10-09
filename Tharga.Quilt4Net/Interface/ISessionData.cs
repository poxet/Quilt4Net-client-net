using System;

namespace Tharga.Quilt4Net.Interface
{
    internal interface ISessionData
    {
        string ClientToken { get; }
        Guid SessionGuid { get; }
        string Environment { get; }
        IApplicationData Application { get; }
        IMachineData Machine { get; }
        IUserData User { get; }
    }
}