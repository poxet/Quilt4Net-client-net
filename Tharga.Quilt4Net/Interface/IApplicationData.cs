using System;

namespace Tharga.Quilt4Net.Interface
{
    internal interface IApplicationData
    {
        string Fingerprint { get; }
        string Name { get; }
        string Version { get; }
        string SupportToolkitNameVersion { get; }
        DateTime? BuildTime { get; }
    }
}