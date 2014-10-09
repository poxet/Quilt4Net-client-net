using System;

namespace Tharga.Quilt4Net
{
    public static class Information
    {
        public static Version Version { get { return Helper.GetApplicationVersion(); } }
        public static string Environment { get { return Configuration.Session.Environment; } }
        public static bool IsClickOnce { get { return Helper.IsClickOnce; } }
        public static DateTime? BuildTime { get { return Helper.GetBuildTime(); } }
        public static string Description { get { return string.Format("{0} {1}{2} {3}", Version, Environment, IsClickOnce ? " (ClickOnce)" : string.Empty, BuildTime); } }
        public static string ToolkitNameVersion { get { return Helper.GetSupportToolkitNameVersion(); } }
        public static string ApplicationName { get { return Helper.GetApplicationName(); } }
        public static string UserName { get { return Helper.GetUserName(); } }
        public static string MachineName { get { return Helper.GetMachineName(); } }
    }
}