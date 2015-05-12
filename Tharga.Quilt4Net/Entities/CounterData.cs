using System;
using System.Collections.Generic;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Entities
{
    internal class CounterData
    {
        public CounterData(Guid id, DateTime clientTime, ISessionData session, string userHandle, string message, IDictionary<string, string> data, List<ICheckpoint> checkpoints)
        {
            Id = id;
            ClientTime = clientTime;
            Session = session;
            UserHandle = userHandle;
            Message = message;
            Data = data;
            Checkpoints = checkpoints;
        }

        public Guid Id { get; private set; }
        public DateTime ClientTime { get; set; }
        public ISessionData Session { get; private set; }
        public string UserHandle { get; private set; }
        public string Message { get; private set; }
        public IDictionary<string, string> Data { get; private set; }
        public List<ICheckpoint> Checkpoints { get; private set; }
    }
}