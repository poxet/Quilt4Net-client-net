using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;

namespace Tharga.Quilt4Net
{
    public static partial class Counter
    {
        public static async Task<CounterResponse> RegisterAsync(CounterInfo counterInfo, string userHandle = null, IDictionary<string, string> data = null)
        {
            return await Task<CounterResponse>.Factory.StartNew(() => Register(counterInfo, userHandle, data));
        }

        public static async Task<CounterResponse> RegisterAsync(string message, string userHandle = null, IDictionary<string, string> data = null)
        {
            return await Task<CounterResponse>.Factory.StartNew(() => Register(message, userHandle, data));
        }

        public static async Task<CounterResponse> RegisterAsync(string message, TimeSpan elapsed, string userHandle = null, IDictionary<string, string> data = null)
        {
            return await Task<CounterResponse>.Factory.StartNew(() => Register(message, elapsed, userHandle, data));
        }
    }
}