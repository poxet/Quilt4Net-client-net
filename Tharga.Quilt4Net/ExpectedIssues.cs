﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;

namespace Tharga.Quilt4Net
{
    public static class ExpectedIssues
    {
        public const int ClientTokenNotSet = 1001;
        public const int UnknownType = 1002;
        public const int CannotSetClientToken = 1003;
        public const int CannotSetEnvironment = 1004;
        public const int ServiceCallError = 1005;
        public const int CannotSetLocation = 1006;
        public const int CannotAutomaticallyRetrieveAssembly = 1007;
        public const int CannotChangeFirstAssembly = 1008;
        public const int FailedToExecutePost = 1009;
        public const int CannotParseIssueLevelException = 1010;
        public const int CannotParseIssueLevelMessage = 1011;
        public const int CannotSetTimeout = 1012;
        public const int Timeout = 1013;

        private static readonly Dictionary<int, string> _data = new Dictionary<int, string>();

        static ExpectedIssues()
        {
            _data.Add(ClientTokenNotSet, "The client token has not been set.");
            _data.Add(UnknownType, "Unknown type.");
            _data.Add(CannotSetClientToken, "Cannot set client token to null, use string.Empty instead.");
            _data.Add(CannotSetEnvironment, "Cannot set Environment to null, use string.Empty instead.");
            _data.Add(ServiceCallError, "Service call error.");
            _data.Add(CannotSetLocation, "Cannot set Location to null, use string.Empty instead.");
            _data.Add(CannotAutomaticallyRetrieveAssembly, "Cannot automatically retrieve entry assembly. Call the Session.Start method and provide the assembly to use.");
            _data.Add(CannotChangeFirstAssembly, "Cannot change first assembly.");
            _data.Add(FailedToExecutePost, "Failed to post WebAPI call. Se inner exceptions for details.");
            _data.Add(CannotParseIssueLevelException, "Cannot parse value for issue level.");
            _data.Add(CannotParseIssueLevelMessage, "Cannot parse value for issue level.");
            _data.Add(CannotSetTimeout, "Cannot set timeout to null.");
            _data.Add(Timeout, "WebAPI call timed out.");
        }

        public static string GetTitle(int code)
        {
            return _data[code];
        }

        public static Exception GetException(int code, Exception innerEception = null)
        {
            Exception exception;
            switch (code)
            {
                case ClientTokenNotSet:
                    exception = new ClientTokenNotSetException(GetMessage(code));
                    break;
                case CannotAutomaticallyRetrieveAssembly:
                case CannotChangeFirstAssembly:
                    exception = new FirstAssemblyException(GetMessage(code));
                    break;
                case CannotSetLocation:
                case CannotSetClientToken:
                case CannotSetEnvironment:
                case CannotSetTimeout:
                    exception = new ConfigurationErrorsException(GetMessage(code), innerEception);
                    break;
                case Timeout:
                    exception = new TimeoutException(GetMessage(code), innerEception);
                    break;
                default:
                    exception = new InvalidOperationException(GetMessage(code), innerEception);
                    break;
            }

            exception.HelpLink = GetHelpLink(code);
            return exception;
        }

        public static WebException GetException(WebException exception)
        {
            var response = exception.Response as HttpWebResponse;
            var serializer = new JavaScriptSerializer();
            dynamic message = exception.Message;
            var dic = new Dictionary<string, string>();
            if (response != null && (response.StatusDescription.StartsWith("{") && response.StatusDescription.EndsWith("}")))
            {
                var d = serializer.DeserializeObject(response.StatusDescription) as dynamic;
                message = d["Message"];
                if (d["Data"] != null)
                {
                    foreach (var data in d["Data"])
                    {
                        dic.Add(data.Key, data.Value);
                    }
                }
            }

            var exp = new WebException(string.Format("{0}{2}{1}", exception.Message, message, Environment.NewLine), exception, exception.Status, exception.Response);
            foreach (var item in dic)
            {
                exp.Data.Add(item.Key, item.Value);
            }

            return exp;
        }

        private static string GetMessage(int code)
        {
            var response = string.Format("{0} Issue code #{1}. Visit {2} for more information", _data[code], code, GetHelpLink(code));
            return response;
        }

        private static string GetHelpLink(int code)
        {
            return string.Format("{1}Help/Details/{0}", code, Configuration.Target.Location);
        }
    }
}