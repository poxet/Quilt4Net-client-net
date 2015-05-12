using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.Interface;
using Tharga.Quilt4Net.Target;

namespace Tharga.Quilt4Net
{
    public static partial class Counter
    {
        public class RegisterCompleteEventArgs : EventArgs
        {
            private readonly bool _isServerOnline;
            private readonly Exception _exception;
            private readonly CounterResponse _counterResponse;

            public RegisterCompleteEventArgs(bool isServerOnline, CounterResponse counterResponse)
            {
                _isServerOnline = isServerOnline;
                _counterResponse = counterResponse;
            }

            public RegisterCompleteEventArgs(bool isServerOnline, Exception exception)
            {
                _isServerOnline = isServerOnline;
                _exception = exception;
            }

            public bool IsServerOnline { get { return _isServerOnline; } }
            public bool Success { get { return _exception == null; } }
            public Exception Exception { get { return _exception; } }
            public CounterResponse CounterResponse { get { return _counterResponse; } }
        }

        public static event EventHandler<RegisterCompleteEventArgs> RegisterCompleteEvent;

        private static void InvokeRegisterComplete(RegisterCompleteEventArgs eventArgs)
        {
            var handler = RegisterCompleteEvent;
            if (handler != null) handler(null, eventArgs);
        }

        public static CounterInfo Start(string message)
        {
            return new CounterInfo(message);
        }

        public static void Checkpoint(CounterInfo counterInfo, string message)
        {
            counterInfo.Step(message);
        }

        public static CounterResponse Register(CounterInfo counterInfo, string userHandle = null, IDictionary<string, string> data = null)
        {
            counterInfo.End();

            if (!Configuration.Enabled) return null;

            var response = RegisterEx(counterInfo.Message, counterInfo.Checkpoints.ToList(), userHandle, data, null);

            if (!response.Success)
                throw new InvalidOperationException("Unable to register counter. Look at inner exception for details.", response.Exception);

            return response.CounterResponse;
        }

        public static void BeginRegister(CounterInfo counterInfo, string userHandle = null, IDictionary<string, string> data = null, Action<RegisterCompleteEventArgs> completeAction = null)
        {
            counterInfo.End();

            if (!Configuration.Enabled) return;
            BeginRegisterEx(counterInfo.Message, counterInfo.Checkpoints.ToList(), userHandle, data, completeAction);
        }

        public static CounterResponse Register(string message, string userHandle = null, IDictionary<string, string> data = null)
        {
            if (!Configuration.Enabled) return null;

            var response = RegisterEx(message, new List<ICheckpoint>(), userHandle, data, null);

            if (!response.Success)
                throw new InvalidOperationException("Unable to register counter. Look at inner exception for details.", response.Exception);

            return response.CounterResponse;
        }

        public static void BeginRegister(string message, string userHandle = null, IDictionary<string, string> data = null, Action<RegisterCompleteEventArgs> completeAction = null)
        {
            if (!Configuration.Enabled) return;
            BeginRegisterEx(message, new List<ICheckpoint>(), userHandle, data, completeAction);
        }

        public static CounterResponse Register(string message, TimeSpan elapsed, string userHandle = null, IDictionary<string, string> data = null)
        {
            if (!Configuration.Enabled) return null;

            var response = RegisterEx(message, new List<ICheckpoint> { new CheckPoint { Elapsed = elapsed } }, userHandle, data, null);

            if (!response.Success)
                throw new InvalidOperationException("Unable to register counter. Look at inner exception for details.", response.Exception);

            return response.CounterResponse;
        }

        public static void BeginRegister(string message, TimeSpan elapsed, string userHandle = null, IDictionary<string, string> data = null, Action<RegisterCompleteEventArgs> completeAction = null)
        {
            if (!Configuration.Enabled) return;
            BeginRegisterEx(message, new List<ICheckpoint> { new CheckPoint { Elapsed = elapsed } }, userHandle, data, completeAction);
        }

        private static void BeginRegisterEx(string message, List<ICheckpoint> checkpoints, string userHandle, IDictionary<string, string> data, Action<RegisterCompleteEventArgs> completeAction)
        {
            Task.Factory.StartNew(() => RegisterEx(message, checkpoints, userHandle, data, completeAction));
        }

        private static RegisterCompleteEventArgs RegisterEx(string message, List<ICheckpoint> checkpoints, string userHandle, IDictionary<string, string> data, Action<RegisterCompleteEventArgs> completeAction)
        {
            RegisterCompleteEventArgs registerCompleteEventArgs = null;
            try
            {
                var target = TargetFactory.Get();
                var response = target.RegisterCounter(new CounterData(Guid.NewGuid(), DateTime.UtcNow, Session.GetSessionData(), userHandle, message, data, checkpoints));

                Session.RegisteredOnServer = true; //TODO: This is true, also if an issue has been returned from the server

                registerCompleteEventArgs = new RegisterCompleteEventArgs(true, response);
            }
            catch (InvalidOperationException exception)
            {
                registerCompleteEventArgs = new RegisterCompleteEventArgs(false, exception);
            }
            catch (Exception exception)
            {
                registerCompleteEventArgs = new RegisterCompleteEventArgs(false, exception);
            }
            finally
            {
                InvokeRegisterComplete(registerCompleteEventArgs);
            }

            if (completeAction != null)
                completeAction.Invoke(registerCompleteEventArgs);

            return registerCompleteEventArgs;
        }
    }
}