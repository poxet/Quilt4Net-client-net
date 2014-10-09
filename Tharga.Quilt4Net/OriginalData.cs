using System.Collections.Generic;
using Tharga.Quilt4Net.Entities;

namespace Tharga.Quilt4Net
{
    public class OriginalData
    {
        internal OriginalData(IssueData issueData)
        {
            ExceptionTypeName = issueData.IssueType.ExceptionTypeName;
            Message = issueData.IssueType.Message;
            StackTrace = issueData.IssueType.StackTrace;
            Data = issueData.Data;
        }

        public string ExceptionTypeName { get; private set; }
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public IDictionary<string, string> Data { get; private set; }
    }
}