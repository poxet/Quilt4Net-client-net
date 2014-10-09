using System;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Entities;
using Tharga.Quilt4Net.Interface;
using ApplicationData = Tharga.Quilt4Net.DataTransfer.ApplicationData;
using IssueType = Tharga.Quilt4Net.DataTransfer.IssueType;
using MachineData = Tharga.Quilt4Net.DataTransfer.MachineData;
using UserData = Tharga.Quilt4Net.DataTransfer.UserData;

namespace Tharga.Quilt4Net
{
    internal static class Converter
    {
        public static DataTransfer.Session ToSessionData(this ISessionData sessionData)
        {
            if (sessionData == null) throw new ArgumentNullException("sessionData", "No session data provided.");

            return new DataTransfer.Session
                {
                    ClientToken = sessionData.ClientToken,
                    SessionGuid = sessionData.SessionGuid,
                    Environment = sessionData.Environment,
                    Application = new ApplicationData
                        {
                            Fingerprint = sessionData.Application.Fingerprint,
                            Name = sessionData.Application.Name,
                            Version = sessionData.Application.Version,
                            SupportToolkitNameVersion = sessionData.Application.SupportToolkitNameVersion,
                            BuildTime = sessionData.Application.BuildTime
                        },
                    Machine = new MachineData
                        {
                            Fingerprint = sessionData.Machine.Fingerprint,
                            Name = sessionData.Machine.Name,
                            Data = sessionData.Machine.Data.ToDictionary(x => x.Key, x => x.Value),
                        },
                    User = new UserData
                        {
                            Fingerprint = sessionData.User.Fingerprint,
                            UserName = sessionData.User.UserName,
                        },
                    //ClientTime = DateTime.UtcNow,
                };
        }

        public static IssueType ToIssueType(this Entities.IssueType item)
        {
            return new IssueType
                {
                    Message = item.Message,
                    StackTrace = item.StackTrace,
                    IssueLevel = item.IssueLevel.ToString(),
                    ExceptionTypeName = item.ExceptionTypeName,
                    Inner = item.Inner != null ? item.Inner.ToIssueType() : null,
                };
        }

        public static RegisterIssueRequest ToIssueData(this IssueData item)
        {
            return new RegisterIssueRequest
            {
                Id = item.Id,
                Session = item.Session.ToSessionData(),
                ClientTime = item.ClientTime,
                VisibleToUser = item.VisibleToUser,
                Data = item.Data,
                IssueType = item.IssueType.ToIssueType(),
                IssueThreadGuid = item.IssueThreadGuid,
                UserHandle = item.UserHandle,
                UserInput = item.UserInput,                
            };
        }
    }
}