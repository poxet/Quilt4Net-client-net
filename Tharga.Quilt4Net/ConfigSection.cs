using System;
using System.Configuration;
using Tharga.Quilt4Net.Target;

namespace Tharga.Quilt4Net
{
    internal class ConfigSection : ConfigurationSection
    {
        private static readonly Lazy<ConfigSection> _instance = new Lazy<ConfigSection>(() => ConfigurationManager.GetSection("Tharga/Quilt4Net") as ConfigSection ?? new ConfigSection());

        private ConfigSection()
        {
            
        }

        public static ConfigSection Instance { get { return _instance.Value; } }

        [ConfigurationProperty("ClientToken", IsRequired = false)]
        public string ClientTokenValue
        {
            get
            {
                var value = base[new ConfigurationProperty("ClientToken", typeof(string), null)];
                return (string)value;
            }
            set
            {
                this["ClientToken"] = value;
            }
        }

        [ConfigurationProperty("Enabled", IsRequired = false, DefaultValue = true)]
        public bool EnabledValue
        {
            get
            {
                return (bool)this["Enabled"];
            }
            set
            {
                this["Enabled"] = value;
            }
        }

        [ConfigurationProperty("UseBuildTime", IsRequired = false, DefaultValue = false)]
        public bool UseBuildTimeValue
        {
            get
            {
                return (bool)this["UseBuildTime"];
            }
            set
            {
                this["UseBuildTime"] = value;
            }
        }

        [ConfigurationProperty("Session")]
        public SessionElement SessionValue
        {
            get
            {
                return (SessionElement)this["Session"];
            }
            set
            {
                this["Session"] = value;
            }
        }

        [ConfigurationProperty("Target")]
        internal TargetElement TargetValue
        {
            get
            {
                return (TargetElement)this["Target"];
            }
            set
            {
                this["Target"] = value;
            }
        }
    }
}