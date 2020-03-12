using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Common
{
    public interface IScheduler
    {
        void Run();
        void Stop();
        void Close();


        void ScheduleElapsed(object sender, ElapsedEventArgs e);
    }
}
