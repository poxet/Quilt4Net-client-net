using System;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class CheckPoint : ICheckpoint
    {
        public int Level { get; set; }
        public string Step { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}