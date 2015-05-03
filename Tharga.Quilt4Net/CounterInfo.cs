using System;
using System.Diagnostics;

namespace Tharga.Quilt4Net
{
    public class CounterInfo
    {
        private readonly string _message;
        private readonly Stopwatch _stopWatch;

        public CounterInfo(string message)
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _message = message;
        }

        public string Message { get { return _message; } }

        public TimeSpan Elapsed
        {
            get
            {
                _stopWatch.Stop();
                return _stopWatch.Elapsed;
            }
        }
    }
}