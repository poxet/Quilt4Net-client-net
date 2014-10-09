using System.Collections.Generic;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Entities
{
    public class MachineData : IMachineData
    {
        private readonly string _fingerprint;
        private readonly string _name;
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>();

        public MachineData(string fingerprint, string name, Dictionary<string, string> data)
        {
            _fingerprint = fingerprint;
            _name = name;
            _data = data;
        }

        public string Fingerprint { get { return _fingerprint; } }
        public string Name { get { return _name; } }
        public IEnumerable<KeyValuePair<string, string>> Data { get { return _data; } }

        public void AddData(string key, string value)
        {
            _data.Add(key, value);
        }

        public void SetData(string key, string value)
        {            
            if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);
        }

        public void RemoveData(string key)
        {
            _data.Remove(key);
        }
    }
}