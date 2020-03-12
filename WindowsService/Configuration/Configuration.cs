using System.Configuration;

namespace WindowsService.Configuration
{
    public class Configuration
    {
        public static ActionQueueConfigSection Get
        {
            get
            {
                var config = ConfigurationManager.GetSection("ActionQueue") as ActionQueueConfigSection;

                if (config == null)
                {
                    throw new ConfigurationErrorsException("Configuration is invalid");
                }

                return config;
            }
        }
    }
}
