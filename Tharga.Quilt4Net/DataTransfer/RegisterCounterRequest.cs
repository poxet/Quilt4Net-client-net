using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class RegisterCounterRequest
    {
        public Guid Id { get; set; }
        public Session Session { get; set; }
        public DateTime ClientTime { get; set; }
        public string UserHandle { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public List<CheckPoint> Checkpoints { get; set; }
    }
}