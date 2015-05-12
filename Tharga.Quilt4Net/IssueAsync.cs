using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;

namespace Tharga.Quilt4Net
{
    public static partial class Issue
    {
        public async static Task<IssueResponse> RegisterAsync(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, bool? visibleToUser = null, string userHandle = null, string userInput = null)
        {
            return await Task<IssueResponse>.Factory.StartNew(() => Register(exception, issueLevel, visibleToUser, userHandle, userInput));
        }

        public async static Task<IssueResponse> RegisterASync(string message, MessageIssueLevel issueLevel, bool? visibleToUser = null, string userHandle = null, string userInput = null, IDictionary<string, string> data = null)
        {
            return await Task<IssueResponse>.Factory.StartNew(() => Register(message, issueLevel, visibleToUser, userHandle, userInput, data));
        }
    }
}