using System;

namespace Tharga.Quilt4Net
{
    internal class ConfigurationException : ApplicationException
    {
        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
}