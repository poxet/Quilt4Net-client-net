using System;
using Tharga.Quilt4Net.Interface;

namespace Tharga.Quilt4Net
{
    public static class Configuration
    {
        private static readonly object SyncRoot = new object();
        private static string _clientToken;
        private static bool? _enabled;
        private static bool? _useBuildTime;
        private static string _applicationName;
        private static string _applicationVersion;

        public static bool Enabled
        {
            get
            {
                if (_enabled != null) return _enabled.Value;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_enabled == null)
                    {
                        _enabled = ConfigSection.Instance.EnabledValue;
                    }
                }

                return _enabled.Value;
            }

            set { _enabled = value; }
        }

        public static string ClientToken
        {
            get
            {
                if (_clientToken != null) return _clientToken;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_clientToken == null)
                    {
                        _clientToken = ConfigSection.Instance.ClientTokenValue;
                    }
                }

                return _clientToken;
            }

            set
            {
                if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetClientToken);
                _clientToken = value;
            }
        }

        public static bool UseBuildTime
        {
            get
            {
                if (_useBuildTime != null) return _useBuildTime.Value;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_useBuildTime == null)
                    {
                        _useBuildTime = ConfigSection.Instance.UseBuildTimeValue;
                    }
                }

                return _useBuildTime.Value;
            }

            set { _useBuildTime = value; }
        }

        public static class Session
        {
            private static string _environment;

            public static string Environment
            {
                get
                {
                    if (_environment != null) return _environment;

                    //If there is no setting, read from config file to populate the value
                    lock (SyncRoot)
                    {
                        if (_environment == null)
                        {
                            _environment = ConfigSection.Instance.SessionValue.Environment ?? string.Empty;
                        }
                    }

                    return _environment;
                }

                set
                {
                    if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetEnvironment);
                    _environment = value;
                }
            }
        }

        public static class Target
        {
            internal enum TargetType
            {
                Service,
                Mock,
                File
            }

            internal static ITarget Instance { get; set; }

            private static string _location;
            private static TargetType? _type;
            private static TimeSpan? _timeout;

            public static string Location
            {
                get
                {
                    if (_location != null) return _location;

                    //If there is no setting, read from config file to populate the value
                    lock (SyncRoot)
                    {
                        if (_location == null)
                        {
                            _location = ConfigSection.Instance.TargetValue.Location ?? string.Empty;
                            if (!_location.EndsWith("/")) _location += "/";
                        }
                    }

                    return _location;
                }

                set
                {
                    if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetLocation);
                    _location = value;
                    if (!_location.EndsWith("/")) _location += "/";
                }
            }

            internal static TargetType Type
            {
                get
                {
                    if (_type != null) return _type.Value;

                    //If there is no setting, read from config file to populate the value
                    lock (SyncRoot)
                    {
                        if (_type == null)
                        {
                            _type = ConfigSection.Instance.TargetValue.Type;
                        }
                    }

                    return _type.Value;
                }

                set { _type = value; }
            }

            public static TimeSpan Timeout
            {
                get
                {
                    if (_timeout != null) return _timeout.Value;

                    //If there is no setting, read from config file to populate the value
                    lock (SyncRoot)
                    {
                        if (_timeout == null)
                        {
                            _timeout = ConfigSection.Instance.TargetValue.Timeout;
                        }
                    }

                    return _timeout.Value;
                }

                set
                {
                    if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetTimeout);
                    _timeout = value;
                }
            }
        }

        internal static string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationName)) _applicationName = Helper.GetApplicationName();
                return _applicationName;
            }

            set { _applicationName = value; }
        }

        internal static string ApplicationVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationVersion)) _applicationVersion = Helper.GetApplicationVersion().ToString();
                return _applicationVersion;
            }

            set { _applicationVersion = value; }
        }
    }
}