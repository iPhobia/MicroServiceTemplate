using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Common.Logger
{
    public interface ILogger
    {
        ILog Log { get; }
    }
}
