namespace Tharga.Quilt4Net.DataTransfer
{
    public class IssueType
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string IssueLevel { get; set; }
        public string ExceptionTypeName { get; set; }
        public IssueType Inner { get; set; }
    }
}