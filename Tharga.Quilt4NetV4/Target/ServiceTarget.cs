using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Target
{
    internal class ServiceTarget : ITarget
    {
        private readonly string _address;

        public ServiceTarget(string address, TimeSpan timeout)
        {
            _address = address;
        }

        public void RegisterSession(ISessionData sessionData)
        {
            var data = new RegisterSessionRequest { Session = sessionData.ToSessionData() };
            ExecuteCommand(data, "session", "register");
        }

        public void EndSession(Guid sessionId)
        {
            ExecuteCommand(sessionId, "session", "end");
        }

        public IssueResponse RegisterIssue(IssueData issueData)
        {
            var data = issueData.ToIssueData();
            var result = ExecuteQuery<IssueResponse, RegisterIssueRequest>(data, "issue", "register");
            return result;
        }

        private TResponse ExecuteQuery<TResponse, T>(T data, string controller, string action)
        {
            var jsonData = string.Empty;
            var address = string.Empty;

            try
            {
                var client = new WebClient();
                var serializer = new JavaScriptSerializer();
                jsonData = serializer.Serialize(data);

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                address = string.Format("{0}/api/{1}/{2}", _address, controller, action);
                var downloadStringTask = client.UploadString(address, jsonData);

                var result = serializer.Deserialize<TResponse>(downloadStringTask);
                return result;
            }
            catch (AggregateException exception)
            {
                throw ExpectedIssues.GetException(ExpectedIssues.FailedToExecutePost, exception.InnerException); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (TimeoutException exception)
            {
                throw ExpectedIssues.GetException(ExpectedIssues.Timeout, exception); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (WebException exception)
            {
                throw ExpectedIssues.GetException(exception); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (Exception exception)
            {
                //exception.AddData("JsonData", jsonData).AddData("Address", address);
                throw;
            }
        }

        private void ExecuteCommand<T>(T data, string controller, string action)
        {
            var jsonData = string.Empty;
            var address = string.Empty;

            try
            {
                var client = new WebClient();
                var serializer = new JavaScriptSerializer();
                jsonData = serializer.Serialize(data);

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                address = string.Format("{0}/api/{1}/{2}", _address, controller, action);
                client.UploadString(address, jsonData);
            }
            catch (AggregateException exception)
            {
                throw ExpectedIssues.GetException(ExpectedIssues.FailedToExecutePost, exception.InnerException); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (TimeoutException exception)
            {
                throw ExpectedIssues.GetException(ExpectedIssues.Timeout, exception); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (WebException exception)
            {
                throw ExpectedIssues.GetException(exception); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (Exception exception)
            {
                //exception.AddData("JsonData", jsonData).AddData("Address", address);
                throw;
            }
        }

        public string ExecuteQuery(string jsonData, string controller, string action)
        {
            var address = string.Empty;

            try
            {
                address = string.Format("{2}/api/{0}/{1}", controller, action, _address);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    streamWriter.Write(jsonData);

                var httpResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    return responseText;
                }
            }
            catch (AggregateException exception)
            {
                throw ExpectedIssues.GetException(ExpectedIssues.FailedToExecutePost, exception.InnerException); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (TimeoutException exception)
            {
                throw ExpectedIssues.GetException(ExpectedIssues.Timeout, exception); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (WebException exception)
            {
                throw ExpectedIssues.GetException(exception); //.AddData("JsonData", jsonData).AddData("Address", address);
            }
            catch (Exception exception)
            {
                //exception.AddData("JsonData", jsonData).AddData("Address", address);
                throw;
            }
        }
    }
}