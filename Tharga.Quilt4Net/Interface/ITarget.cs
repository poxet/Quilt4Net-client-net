using System;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;

namespace Tharga.Quilt4Net.Interface
{
    internal interface ITarget
    {
        void RegisterSession(ISessionData sessionData);
        void EndSession(Guid sessionId);
        IssueResponse RegisterIssue(IssueData issueData);
        CounterResponse RegisterCounter(CounterData counterData);
    }
}