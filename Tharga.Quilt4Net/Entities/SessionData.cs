using System;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Entities
{
    internal class SessionData : ISessionData
    {
        public SessionData(string clientToken, Guid sessionGuid, DateTime clientStartTime, string environment, IApplicationData application, IMachineData machine, IUserData user)
        {
            ClientToken = clientToken;
            SessionGuid = sessionGuid;
            ClientStartTime = clientStartTime;
            Environment = environment;
            Application = application;
            Machine = machine;
            User = user;
        }

        public string ClientToken { get; private set; }
        public Guid SessionGuid { get; private set; }
        public DateTime ClientStartTime { get; private set; }
        public string Environment { get; private set; }
        public IApplicationData Application { get; private set; }
        public IMachineData Machine { get; private set; }
        public IUserData User { get; private set; }
    }
}