using System.Reflection;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;

namespace Tharga.Quilt4Net
{
    public static partial class Session
    {
        public static async Task<SessionResponse> RegisterAsync(Assembly firstAssembly)
        {
            return await Task<SessionResponse>.Factory.StartNew(() => Register(firstAssembly));
        }

        public static async Task<SessionResponse> RegisterAsync()
        {
            return await Task<SessionResponse>.Factory.StartNew(Register);
        }
    }
}