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

        public static void Register(string message, string userHandle = null, IDictionary<string, string> data = null)
        {
        }

        public static void Register(CounterInfo counterInfo, string userHandle = null, IDictionary<string, string> data = null)
        {
        }

        public static void Register(string message, TimeSpan duration, string userHandle = null, IDictionary<string, string> data = null)
        {
        }
    }
}