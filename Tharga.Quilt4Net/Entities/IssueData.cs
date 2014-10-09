using System;
using System.Collections.Generic;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Entities
{
    internal class IssueData
    {
        public IssueData(Guid id, DateTime clientTime, ISessionData session, bool? visibleToUser, IDictionary<string, string> data, IssueType issueType, Guid? issueThreadGuid, string userHandle, string userInput)
        {
            Id = id;
            ClientTime = clientTime;
            Session = session;
            VisibleToUser = visibleToUser;
            Data = data;
            IssueType = issueType;
            IssueThreadGuid = issueThreadGuid;
            UserHandle = userHandle;
            UserInput = userInput;
        }

        public Guid Id { get; private set; }
        public DateTime ClientTime { get; set; }
        public ISessionData Session { get; private set; }
        public bool? VisibleToUser { get; private set; }
        public IDictionary<string, string> Data { get; private set; }
        public IssueType IssueType { get; private set; }
        public Guid? IssueThreadGuid { get; private set; }
        public string UserHandle { get; private set; }
        public string UserInput { get; private set; }
    }
}