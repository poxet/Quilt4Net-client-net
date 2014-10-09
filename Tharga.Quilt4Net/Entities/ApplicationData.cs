using System;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Entities
{
    internal class ApplicationData : IApplicationData
    {
        public ApplicationData(string fingerprint, string name, string version, string supportToolkitNameVersion, DateTime? buildTime)
        {
            Fingerprint = fingerprint;
            Name = name;
            Version = version;
            SupportToolkitNameVersion = supportToolkitNameVersion;
            BuildTime = buildTime;
        }

        public string Fingerprint { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string SupportToolkitNameVersion { get; private set; }
        public DateTime? BuildTime { get; private set; }
    }
}