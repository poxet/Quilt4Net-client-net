using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.Target;
using IssueType = Tharga.Quilt4Net.Entities.IssueType;

namespace Tharga.Quilt4Net
{
    public static partial class Issue
    {
        public class RegisterCompleteEventArgs : EventArgs
        {
            private readonly bool _isServerOnline;
            private readonly IssueResponse _issueResponse;
            private readonly Exception _exception;
            private readonly OriginalData _originalData;

            public RegisterCompleteEventArgs(bool isServerOnline, IssueResponse issueResponse, OriginalData originalData)
            {
                _isServerOnline = isServerOnline;
                _issueResponse = issueResponse;
                _originalData = originalData;
            }

            public RegisterCompleteEventArgs(bool isServerOnline, Exception exception, OriginalData originalData)
            {
                _isServerOnline = isServerOnline;
                _exception = exception;
                _originalData = originalData;
            }

            public bool IsServerOnline { get { return _isServerOnline; } }
            public bool Success { get { return _exception == null; } }
            public IssueResponse IssueResponse { get { return _issueResponse; } }
            public Exception Exception { get { return _exception; } }
            public OriginalData OriginalData { get { return _originalData; } }
        }

        public static event EventHandler<RegisterCompleteEventArgs> RegisterCompleteEvent;

        private static void InvokeRegisterComplete(RegisterCompleteEventArgs eventArgs)
        {
            var handler = RegisterCompleteEvent;
            if (handler != null) handler(null, eventArgs);
        }

        public enum MessageIssueLevel
        {
            Information,
            Warning,
            Error,
        }

        public enum ExceptionIssueLevel
        {
            Warning,
            Error,
        }

        public static IssueResponse Register(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, bool? visibleToUser = null, string userHandle = null, string userInput = null)
        {
            if (!Configuration.Enabled) return null;

            var issueData = PrepareIssueData(exception, issueLevel, visibleToUser, userHandle, userInput);
            var response = RegisterEx(issueData, null);

            if (!response.Success) throw new InvalidOperationException("Unable to register exception. Look at inner exception for details.", response.Exception);

            return response.IssueResponse;
        }

        public static void BeginRegister(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, bool? visibleToUser = null, string userHandle = null, string userInput = null, Action<RegisterCompleteEventArgs> completeAction = null)
        {
            if (!Configuration.Enabled) return;
            Func<IssueData> issueData = () => PrepareIssueData(exception, issueLevel, visibleToUser, userHandle, userInput);
            BeginRegisterEx(issueData, completeAction);
        }

        private static IssueData PrepareIssueData(Exception exception, ExceptionIssueLevel issueLevel, bool? visibleToUser, string userHandle, string userInput)
        {
            var issueThreadGuid = HandleIssueThreadGuid(exception);
            var data = exception.Data.Cast<DictionaryEntry>().Where(x => x.Value != null).ToDictionary(item => item.Key.ToString(), item => item.Value.ToString());
            var issueType = new IssueType(exception, issueLevel.ToIssueLevel());
            var issueData = new IssueData(Guid.NewGuid(), DateTime.UtcNow, Session.GetSessionData(), visibleToUser, data, issueType, issueThreadGuid, userHandle, userInput);
            return issueData;
        }

        public static IssueResponse Register(string message, MessageIssueLevel issueLevel, bool? visibleToUser = null, string userHandle = null, string userInput = null, IDictionary<string, string> data = null)
        {
            if (!Configuration.Enabled) return null;
            var issueData = PrepareIssueData(message, issueLevel, visibleToUser, userHandle, userInput, data);
            var response = RegisterEx(issueData, null);

            if (!response.Success) throw new InvalidOperationException("Unable to register exception. Look at inner exception for details.", response.Exception);

            return response.IssueResponse;
        }

        public static void BeginRegister(string message, MessageIssueLevel issueLevel, bool? visibleToUser = null, string userHandle = null, string userInput = null, IDictionary<string, string> data = null, Action<RegisterCompleteEventArgs> completeAction = null)
        {
            if (!Configuration.Enabled) return;
            Func<IssueData> issueData = () => PrepareIssueData(message, issueLevel, visibleToUser, userHandle, userInput, data);
            BeginRegisterEx(issueData, completeAction);
        }

        private static IssueData PrepareIssueData(string message, MessageIssueLevel issueLevel, bool? visibleToUser, string userHandle, string userInput, IDictionary<string, string> data)
        {
            var issueType = new IssueType(message, issueLevel.ToIssueLevel());
            var issueData = new IssueData(Guid.NewGuid(), DateTime.UtcNow, Session.GetSessionData(), visibleToUser, data, issueType, Guid.NewGuid(), userHandle, userInput);
            return issueData;
        }

        private static Guid HandleIssueThreadGuid(Exception exception)
        {
            var refItg = Guid.NewGuid();

            if (exception == null) return refItg;

            if (!exception.Data.Contains("IssueThreadGuid"))
            {
                exception.Data.Add("IssueThreadGuid", refItg);
            }
            else
            {
                Guid result;
                if (Guid.TryParse(exception.Data["IssueThreadGuid"].ToString(), out result))
                {
                    refItg = result;
                }
                else
                {
                    //NOTE: When there is a general message/warning event. Fire this information.
                    //Provided IssueThreadGuid cannot be parsed as Guid. Apply a new valid value.
                    exception.Data["IssueThreadGuid"] = refItg;
                }
            }

            return refItg;
        }

        private static RegisterCompleteEventArgs RegisterEx(IssueData issueData, Action<RegisterCompleteEventArgs> completeAction)
        {
            RegisterCompleteEventArgs registerCompleteEventArgs = null;
            try
            {
                var target = TargetFactory.Get();
                var reponse = target.RegisterIssue(issueData);

                Session.RegisteredOnServer = true; //TODO: This is true, also if an issue has been returned from the server

                //TODO: Make it possible to append a user comment on an issue (Ither on the initial call, or afterwords as a second call)
                registerCompleteEventArgs = new RegisterCompleteEventArgs(true, reponse, new OriginalData(issueData));
            }
            catch (InvalidOperationException exception)
            {
                registerCompleteEventArgs = new RegisterCompleteEventArgs(false, exception, new OriginalData(issueData));
            }
            catch (Exception exception)
            {
                registerCompleteEventArgs = new RegisterCompleteEventArgs(false, exception, new OriginalData(issueData));
            }
            finally
            {
                InvokeRegisterComplete(registerCompleteEventArgs);
            }

            if (completeAction != null) completeAction.Invoke(registerCompleteEventArgs);

            return registerCompleteEventArgs;
        }

        private static void BeginRegisterEx(Func<IssueData> issueData, Action<RegisterCompleteEventArgs> completeAction) { Task.Factory.StartNew(() => RegisterEx(issueData(), completeAction)); }
    }
}