using System;

namespace Tharga.Quilt4Net.Interface
{
    internal interface ICheckpoint
    {
        int Level { get; }
        string Step { get; }
        TimeSpan Elapsed { get; }
    }
}