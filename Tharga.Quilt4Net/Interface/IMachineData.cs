using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interface
{
    public interface IMachineData
    {
        string Fingerprint { get; }
        string Name { get; }
        IEnumerable<KeyValuePair<string, string>> Data { get; }
    }
}