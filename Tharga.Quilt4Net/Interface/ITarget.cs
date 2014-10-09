using System;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.DataTransfer;

namespace Tharga.Quilt4Net.Interface
{
    internal interface ITarget
    {
        void RegisterSession(ISessionData sessionData);
        void EndSession(Guid sessionId);
        IssueResponse RegisterIssue(IssueData issueData);
    }
}