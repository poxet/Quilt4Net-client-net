using System;
using Tharga.Quilt4Net.Entities;

namespace Tharga.Quilt4Net
{
    internal static class IssueLevelExtension
    {
        internal static IssueLevel ToIssueLevel(this Issue.ExceptionIssueLevel issueLevel)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw ExpectedIssues.GetException(ExpectedIssues.CannotParseIssueLevelException).AddData("IssueLevel", issueLevel);

            return il;
        }

        internal static IssueLevel ToIssueLevel(this Issue.MessageIssueLevel issueLevel)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw ExpectedIssues.GetException(ExpectedIssues.CannotParseIssueLevelMessage).AddData("issueLevel", issueLevel);

            return il;
        }
    }
}