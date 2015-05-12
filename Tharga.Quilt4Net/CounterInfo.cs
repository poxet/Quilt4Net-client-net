using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net
{
    public class CounterInfo
    {
        private readonly string _message;
        private readonly Stopwatch _stopWatch;
        private readonly Dictionary<string, TimeSpan> _checkpoints = new Dictionary<string, TimeSpan>();
        private readonly List<CounterInfo> _subCounters = new List<CounterInfo>();

        public CounterInfo(string message)
        {
            _message = message;
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public string Message { get { return _message; } }
        public TimeSpan Elapsed { get { return _stopWatch.Elapsed; } }
        internal IEnumerable<ICheckpoint> Checkpoints { get { return GetCheckpoints(0); } }

        public void Step(string message)
        {
            _checkpoints.Add(message, _stopWatch.Elapsed);
        }

        public void Step(CounterInfo subCounter)
        {
            _subCounters.Add(subCounter);
        }

        internal void End()
        {
            if (_stopWatch.IsRunning)
            {
                _stopWatch.Stop();
                _checkpoints.Add("End", _stopWatch.Elapsed);
            }
        }

        private IEnumerable<ICheckpoint> GetCheckpoints(int level)
        {
            foreach (var item in _checkpoints)
            {
                yield return new CheckPoint { Level = level, Elapsed = item.Value, Step = item.Key };
            }

            foreach (var counter in _subCounters)
            {
                var items = counter.GetCheckpoints(++level);
                foreach (var item in items) yield return item;
            }
        }
    }
}