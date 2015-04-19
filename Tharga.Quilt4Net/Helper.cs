using System;
using System.Deployment.Application;
using System.IO;
using System.Management;
using System.Reflection;
using System.Threading;

namespace Tharga.Quilt4Net
{
    internal static class Helper
    {
        private static readonly object SyncRoot = new object();
        private static Assembly _firstAssembly;

        public static string GetApplicationFingerprint()
        {
            var clientToken = GetClientToken();
            return string.Format("AI1:{0}", string.Format("{0}{1}{2}{3}{4}", Configuration.ApplicationName, Configuration.ApplicationVersion, GetSupportToolkitNameVersion(), clientToken, GetBuildTime()).ToMd5Hash());
        }

        internal static string GetClientToken()
        {
            var clientToken = Configuration.ClientToken;
            if (string.IsNullOrEmpty(clientToken)) throw ExpectedIssues.GetException(ExpectedIssues.ClientTokenNotSet);
            return clientToken;
        }

        internal static string GetApplicationName()
        {
            return GetFirstAssembly().GetName().Name;
        }

        internal static Version GetApplicationVersion()
        {
            var assemblyVersion = GetFirstAssembly().GetName().Version;
            var clickOnceVersion = (Version)null;

            if (IsClickOnce) clickOnceVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            return clickOnceVersion ?? assemblyVersion;
        }

        internal static bool IsClickOnce
        {
            get { return ApplicationDeployment.IsNetworkDeployed; }
        }

        internal static string GetSupportToolkitNameVersion()
        {
            var toolkitName = Assembly.GetExecutingAssembly().GetName();
            return string.Format("{0} {1}", toolkitName.Name, toolkitName.Version);
        }

        private static Assembly GetFirstAssembly()
        {
            if (_firstAssembly == null)
            {
                lock (SyncRoot)
                {
                    if (_firstAssembly == null)
                    {
                        _firstAssembly = Assembly.GetEntryAssembly();
                        if (_firstAssembly == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotAutomaticallyRetrieveAssembly);
                    }
                }
            }

            return _firstAssembly;
        }

        internal static void SetFirstAssembly(Assembly firstAssembly)
        {
            if (_firstAssembly == null)
            {
                lock (SyncRoot)
                {
                    if (_firstAssembly == null)
                    {
                        _firstAssembly = firstAssembly;
                    }
                }
            }

            if (_firstAssembly != firstAssembly) throw ExpectedIssues.GetException(ExpectedIssues.CannotChangeFirstAssembly).AddData("From", _firstAssembly.GetName().FullName).AddData("To", firstAssembly.GetName().FullName);
        }

        public static string GetMachineFingerprint()
        {
            return string.Format("MI1:{0}", string.Format("{0}{1}{2}", GetCpuId(), GetDriveSerial(), GetMachineName()).ToMd5Hash());
        }

        internal static string GetMachineName()
        {
            return Environment.MachineName;
        }

        internal static string GetOsName()
        {
            var response = "Unknown";
            try
            {
                const string query = "SELECT * FROM Win32_OperatingSystem";
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject info in searcher.Get())
                    {
                        try
                        {
                            response = info["Caption"].ToString();
                            break;
                        }
                        catch (Exception exception)
                        {
                            System.Diagnostics.Debug.WriteLine(exception.Message);
                            response = "N/A";
                        }
                    }
                }
            }
            catch
            {
                try
                {
                    response = Environment.OSVersion.ToString();
                }
                catch
                {
                    response = "N/A";
                }
            }

            return response;
        }

        internal static string GetScreen()
        {
            var response = "Unknown";
            try
            {
                const string GraphicsQuery = "SELECT * FROM Win32_VideoController";
                using (var graphics_searcher = new ManagementObjectSearcher(GraphicsQuery))
                {
                    foreach (ManagementObject info in graphics_searcher.Get())
                    {
                        try
                        {
                            //response = info["Name"] + " (" + info["CurrentHorizontalResolution"] + " x " + info["CurrentVerticalResolution"] + ")";
                            response = string.Format("{0}x{1}", info["CurrentHorizontalResolution"], info["CurrentVerticalResolution"]);
                            break;
                        }
                        catch (Exception exception)
                        {
                            System.Diagnostics.Debug.WriteLine(exception.Message);
                            response = "N/A";
                        }
                    }
                }
            }
            catch
            {
                response = "N/A";
            }

            return response;
        }

        private static string GetCpuId()
        {
            var cpuInfo = "Unknown";
            try
            {
                var mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (string.IsNullOrEmpty(cpuInfo))
                    {
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
            }
            catch
            {
                cpuInfo = "N/A";
            }

            return cpuInfo;
        }

        internal static string GetModel()
        {
            var model = string.Empty;
            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (string.IsNullOrEmpty(model))
                {
                    model = mo.Properties["Name"].Value.ToString();
                    break;
                }
            }

            return model;
        }

        private static string GetDriveSerial()
        {
            var driveSerial = string.Empty;

            var drives = Directory.GetLogicalDrives();
            foreach (var drive in drives)
            {
                if (!drive.StartsWith("A") && !drive.StartsWith("B"))
                {
                    driveSerial = GetHarddriveId(drive[0]);
                    if (!string.IsNullOrEmpty(driveSerial)) break;
                }
            }

            return driveSerial;
        }

        private static string GetHarddriveId(char drive)
        {
            var dsk = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", drive));
            dsk.Get();
            return dsk["VolumeSerialNumber"].ToString();
        }

        public static string GetUserFingerprint()
        {
            return string.Format("UI1:{0}", string.Format("{0}{1}", GetUserSid(), GetUserName()).ToMd5Hash());
        }

        internal static string GetUserName()
        {
            //TODO: Does not work when using the '\' so now I tried with '-'. Perhaps '\\' will work.
            return string.Format(@"{0}-{1}", Environment.UserDomainName, Environment.UserName);
        }

        private static string GetUserSid()
        {
            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (currentUser == null || currentUser.User == null || currentUser.User.AccountDomainSid == null) return "NULL";

            return currentUser.User.AccountDomainSid.ToString();
        }

        public static DateTime? GetBuildTime()
        {
            if (!Configuration.UseBuildTime) return null;

            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;
            FileStream s = null;
            var b = new byte[2048];

            try
            {
                var filePath = GetFirstAssembly().Location;
                s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null) s.Close();
            }

            var i = BitConverter.ToInt32(b, PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(b, i + LinkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);

            return dt;
        }

        public static string GetTimeZone()
        {
            try
            {
                var timeZone = TimeZoneInfo.Local;
                return timeZone.Id;
            }
            catch
            {
                return "N/A";
            }
        }

        public static string GetLanguage()
        {
            try
            {
                var currentCulture = Thread.CurrentThread.CurrentCulture;
                return currentCulture.Name;
            }
            catch
            {
                return "N/A";
            }
        }
    }
}