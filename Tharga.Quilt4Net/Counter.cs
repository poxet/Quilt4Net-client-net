using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net
{
    public static class Counter
    {
        public static CounterInfo Start(string message)
        {
            return new CounterInfo(message);
        }

        public static void Register(CounterInfo counterInfo, string userHandle = null, IDictionary<string, string> data = null)
        {
            RegisterEx(counterInfo.Message, counterInfo.Elapsed, userHandle, data);
        }

        public static void Register(string message, string userHandle = null, IDictionary<string, string> data = null)
        {
            RegisterEx(message, null, userHandle, data);
        }

        public static void Register(string message, TimeSpan elapsed, string userHandle = null, IDictionary<string, string> data = null)
        {
            RegisterEx(message, elapsed, userHandle, data);
        }

        private static void RegisterEx(string message, TimeSpan? elapsed, string userHandle = null, IDictionary<string, string> data = null)
        {
            //TODO: Send to server
        }
    }
}