using System;

namespace Tharga.Quilt4Net
{
    internal class ClientTokenNotSetException : ApplicationException
    {
        public ClientTokenNotSetException(string message)
            : base(message)
        {
        }
    }
}