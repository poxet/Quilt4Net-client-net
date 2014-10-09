using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Entities
{
    internal class UserData : IUserData
    {
        public UserData(string fingerprint, string userName)
        {
            Fingerprint = fingerprint;
            UserName = userName;
        }

        public string Fingerprint { get; private set; }
        public string UserName { get; private set; }
    }
}