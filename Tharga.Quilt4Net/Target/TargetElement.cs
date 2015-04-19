using System;
using System.Configuration;

namespace Tharga.Quilt4Net.Target
{
    internal class TargetElement : ConfigurationElement
    {
        private TargetElement()
        {
        }

        [ConfigurationProperty("Type", IsRequired = false)]
        internal Configuration.Target.TargetType Type
        {
            get
            {
                var configurationProperty = new ConfigurationProperty("Type", typeof(Configuration.Target.TargetType), null);
                var prop = base[configurationProperty];
                if (prop == null) return Configuration.Target.TargetType.Service;
                return (Configuration.Target.TargetType)prop;
            }

            set { this["Type"] = value; }
        }

        [ConfigurationProperty("Location", IsRequired = false)]
        public string Location
        {
            get
            {
                var configurationProperty = new ConfigurationProperty("Location", typeof(string), null);
                return (string)base[configurationProperty] ?? "https://www.Quilt4Net.com/";
            }

            set { this["Location"] = value; }
        }

        [ConfigurationProperty("Timeout", IsRequired = false)]
        public TimeSpan Timeout
        {
            get
            {
                var configurationProperty = new ConfigurationProperty("Timeout", typeof(TimeSpan?), null);
                return (TimeSpan?)base[configurationProperty] ?? new TimeSpan(0, 0, 100);
            }

            set { this["Timeout"] = value; }
        }
    }
}