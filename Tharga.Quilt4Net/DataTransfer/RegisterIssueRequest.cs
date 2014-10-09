using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class RegisterIssueRequest
    {
        public Guid Id { get; set; }
        public Session Session { get; set; }
        public DateTime ClientTime { get; set; }
        public bool? VisibleToUser { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public IssueType IssueType { get; set; }
        public Guid? IssueThreadGuid { get; set; }
        public string UserHandle { get; set; }
        public string UserInput { get; set; }
    }
}