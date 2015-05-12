using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net.Target
{
    internal class ServiceTarget : ITarget
    {
        private readonly string _address;
        private readonly TimeSpan _timeout;

        public ServiceTarget(string address, TimeSpan timeout)
        {
            _address = address;
            _timeout = timeout;
        }

        public void RegisterSession(ISessionData sessionData)
        {
            var data = new RegisterSessionRequest { Session = sessionData.ToSessionData() };

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<RegisterSessionRequest>(data, jsonFormatter);

            ExecuteCommand(content, "session", "register");
        }

        public void EndSession(Guid sessionId)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<Guid>(sessionId, jsonFormatter);

            ExecuteCommand(content, "session", "end");
        }

        public IssueResponse RegisterIssue(IssueData issueData)
        {
            var data = issueData.ToIssueData();

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<RegisterIssueRequest>(data, jsonFormatter);

            var x = ExecuteQuery<RegisterIssueRequest, RegisterIssueResponse>(content, "issue", "register");
            var response = new IssueResponse
            {
                IssueTypeTicket = x.IssueTypeTicket,
                IssueInstanceTicket = x.IssueInstanceTicket,
                IsOfficial = x.IsOfficial,
                ResponseMessage = x.ResponseMessage,
            };
            return response;
        }

        public CounterResponse RegisterCounter(CounterData counterData)
        {
            var data = counterData.ToCounterData();

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<RegisterCounterRequest>(data, jsonFormatter);

            var x = ExecuteQuery<RegisterCounterRequest, RegisterCounterResponse>(content, "counter", "register");
            var response = new CounterResponse();
            return response;
        }

        private TResult ExecuteQuery<T, TResult>(ObjectContent<T> content, string controller, string action)
        {
            try
            {
                var client = GetHttpClient();
                var response = client.PostAsync(string.Format("api/{0}/{1}", controller, action), content);
                if (!response.Wait(_timeout))
                    throw new TimeoutException("The WebAPI call exceeded the allotted time.").AddData("Timeout", _timeout.ToString());
                if (!response.Result.IsSuccessStatusCode)
                    throw ExpectedIssues.GetException(ExpectedIssues.ServiceCallError).AddData("StatusCode", (int)response.Result.StatusCode).AddData("StatusCodeName", response.Result.StatusCode).AddData("ReasonPhrase", response.Result.ReasonPhrase);

                var result = response.Result.Content.ReadAsAsync<TResult>().Result;
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

        private void ExecuteCommand<T>(ObjectContent<T> content, string controller, string action)
        {
            try
            {
                var client = GetHttpClient();
                var response = client.PostAsync(string.Format("api/{0}/{1}", controller, action), content);
                if (!response.Wait(_timeout))
                    throw new TimeoutException("The WebAPI call exceeded the allotted time.").AddData("Timeout", _timeout.ToString());
                if (!response.Result.IsSuccessStatusCode)
                    throw ExpectedIssues.GetException(ExpectedIssues.ServiceCallError).AddData("StatusCode", (int)response.Result.StatusCode).AddData("StatusCodeName", response.Result.StatusCode).AddData("ReasonPhrase", response.Result.ReasonPhrase);
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
            try
            {
                var address = string.Format("{2}/api/{0}/{1}", controller, action, _address);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(address);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))                
                    streamWriter.Write(jsonData);

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
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

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_address) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = _timeout;
            return client;
        }
    }
}