using System.Configuration;

namespace WindowsService.Configuration
{
    public class ActionQueueSettings : ConfigurationElement
    {
        [ConfigurationProperty("ActionQueueProcessingInterval", IsKey = true, IsRequired = false, DefaultValue = 1)]
        public int ActionQueueProcessingInterval
        {
            get
            {
                return (int)this["ActionQueueProcessingInterval"];
            }
        }
    }
}
