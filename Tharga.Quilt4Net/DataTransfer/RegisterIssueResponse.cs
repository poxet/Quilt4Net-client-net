namespace Tharga.Quilt4Net.DataTransfer
{
    public class RegisterIssueResponse
    {
        public string IssueTypeTicket { get; set; }
        public string IssueInstanceTicket { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsOfficial { get; set; }
    }
}