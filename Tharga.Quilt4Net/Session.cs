using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.Interface;
using Tharga.Quilt4Net.Target;
using ApplicationData = Tharga.Quilt4Net.Entities.ApplicationData;
using MachineData = Tharga.Quilt4Net.Entities.MachineData;
using UserData = Tharga.Quilt4Net.Entities.UserData;

namespace Tharga.Quilt4Net
{
    public static class Session
    {
        private static readonly object SyncRoot = new object();
        private static ISessionData _sessionData;
        internal static bool _registeredOnServer;
        private static DateTime	_clientStartTime = DateTime.UtcNow;

        public static MachineData Machine { get { return GetSessionData().Machine as MachineData; } }
        public static DateTime ClientStartTime { get { return _clientStartTime; } }
        public static bool RegisteredOnServer { get { return _registeredOnServer; } }

        public class RegisterCompleteEventArgs : EventArgs
        {
            private readonly bool _isServerOnline;
            private readonly Exception _exception;

            public RegisterCompleteEventArgs(bool isServerOnline)
            {
                _isServerOnline = isServerOnline;
            }

            public RegisterCompleteEventArgs(bool isServerOnline, Exception exception)
            {
                _isServerOnline = isServerOnline;
                _exception = exception;
            }

            public bool IsServerOnline { get { return _isServerOnline; } }
            public bool Success { get { return _exception == null; } }
            public Exception Exception { get { return _exception; } }
        }

        public static event EventHandler<RegisterCompleteEventArgs> RegisterCompleteEvent;

        private static void InvokeRegisterComplete(RegisterCompleteEventArgs eventArgs)
        {
            var handler = RegisterCompleteEvent;
            if (handler != null) handler(null, eventArgs);
        }

        public static void BeginRegister(Assembly firstAssembly)
        {
            Helper.SetFirstAssembly(firstAssembly);
            BeginRegister();
        }

        public static SessionResponse Register(Assembly firstAssembly)
        {
            Helper.SetFirstAssembly(firstAssembly);
            return Register();
        }

        public static void BeginRegister()
        {
            BeginRegisterEx();
        }

        public static SessionResponse Register()
        {
            if (!Configuration.Enabled)
                return null;

            var response = RegisterEx(GetSessionData);

            if (!response.Success)
                throw new InvalidOperationException("Unable to register session. Look at inner exception for details.", response.Exception);

            var result = new SessionResponse(response.IsServerOnline, response.Exception);
            return result;
        }

        private static void BeginRegisterEx()
        {
            if (!Configuration.Enabled) return;

            Helper.GetClientToken();
            Task.Factory.StartNew(() => RegisterEx(GetSessionData));
        }

        internal static ISessionData GetSessionData()
        {
            AssureSessionData();
            return _sessionData;
        }

        private static void AssureSessionData()
        {
            if (_sessionData != null)
                return;

            lock (SyncRoot)
            {
                if (_sessionData != null)
                    return;

                _sessionData = CreateSessionData();
            }
        }

        private static ISessionData CreateSessionData()
        {
            var applicationData = new ApplicationData(Helper.GetApplicationFingerprint(), Configuration.ApplicationName, Configuration.ApplicationVersion, Helper.GetSupportToolkitNameVersion(), Helper.GetBuildTime());
            var data = new Dictionary<string, string>
                       {
                           { "OsName", Helper.GetOsName() },
                           { "Model", Helper.GetModel() },
                           { "Type", "Desktop" },
                           { "Screen", Helper.GetScreen() },
                           { "TimeZone", Helper.GetTimeZone() },
                           { "Language", Helper.GetLanguage() }
                       };

            var machineData = new MachineData(Helper.GetMachineFingerprint(), Helper.GetMachineName(), data );
            var userData = new UserData(Helper.GetUserFingerprint(), Helper.GetUserName());
            var sessionData = new SessionData(Configuration.ClientToken, Guid.NewGuid(), _clientStartTime, Configuration.Session.Environment, applicationData, machineData, userData);
            return sessionData;
        }

        public static void End()
        {
            if (!Configuration.Enabled) 
                return;

            if (_sessionData == null || !_registeredOnServer)
                return;

            Guid sessionGuid;
            lock (SyncRoot)
            {
                if (_sessionData == null) 
                    return;

                sessionGuid = _sessionData.SessionGuid;
                _sessionData = null;
            }

            EndSessionEx(sessionGuid);
        }

        private static void EndSessionEx(Guid sessionId)
        {
            var target = TargetFactory.Get();
            target.EndSession(sessionId);
        }

        private static RegisterCompleteEventArgs RegisterEx(Func<ISessionData> data)
        {
            RegisterCompleteEventArgs registerCompleteEventArgs = null;
            try
            {
                AssureSessionData();

                var target = TargetFactory.Get();
                target.RegisterSession(data());

                registerCompleteEventArgs = new RegisterCompleteEventArgs(true);
                _registeredOnServer = true; //TODO: This is true, also if an issue has been returned from the server. (Even though the session was never started explicitly)
            }
            catch (InvalidOperationException exception)
            {
                registerCompleteEventArgs = new RegisterCompleteEventArgs(true, exception);
            }
            catch (Exception exception)
            {
                registerCompleteEventArgs = new RegisterCompleteEventArgs(false, exception);
            }
            finally
            {
                InvokeRegisterComplete(registerCompleteEventArgs);
            }

            return registerCompleteEventArgs;
        }
    }
}