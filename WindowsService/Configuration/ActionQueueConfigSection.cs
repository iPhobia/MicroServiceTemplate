using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService.Configuration
{
    public class ActionQueueConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("ActionQueueConfig", IsKey = false)]
        public ActionQueueSettings ActivationQueue
        {
            get { return (ActionQueueSettings)this["ActionQueueConfig"]; }
        }
    }
}
