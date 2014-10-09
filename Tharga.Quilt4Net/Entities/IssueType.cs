using System;

namespace Tharga.Quilt4Net.Entities
{
    internal class IssueType
    {
        public IssueType(Exception exception, IssueLevel issueLevel)
        {
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            IssueLevel = issueLevel;
            ExceptionTypeName = exception.GetType().ToString();
            Inner = exception.InnerException != null ? new IssueType(exception.InnerException, issueLevel) : null;
        }

        public IssueType(string message, IssueLevel issueLevel)
        {
            Message = message;
            StackTrace = null;
            IssueLevel = issueLevel;
            ExceptionTypeName = null;
            Inner = null;
        }

        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public IssueLevel IssueLevel { get; private set; }
        public string ExceptionTypeName { get; private set; }
        public IssueType Inner { get; private set; }
    }
}