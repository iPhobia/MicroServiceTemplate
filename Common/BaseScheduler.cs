using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;

namespace Common
{
    public class BaseScheduler
    {
        private const int MaxInterval = 20160 * 60 * 1000; // 2 weeks
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BaseScheduler(double interval)
        {
            _interval = interval;
            _log.InfoFormat("Interval:{0}", _interval);

            if (_interval > MaxInterval)
            {
                _log.Error("Interval is greater than max value (2 weeks)");
                _interval = 0;
            }
        }


        private Timer _schedule;

        private double _interval;
        private DateTime? _executeDate;

        public Boolean IsTimerActive
        {
            get { return _schedule != null; }
        }
        public double Interval { get { return _interval; } }
        public Timer Schedule { get { return _schedule; } }
        public DateTime? ExecuteDate
        {
            get { return _executeDate; }
        }

        public event Action<object, ElapsedEventArgs> Elapsed;

        public void Run(Action<object, ElapsedEventArgs> elapsed)
        {
            _log.Info("Run internal scheduler");
            Elapsed = elapsed;

            if (_schedule != null || _interval <= 0)
            {
                _log.Error("Schedule has already been created or Interval equals 0");
                return;
            }

            _log.Info("Initiate timer");
            _schedule = new Timer
            {
                AutoReset = true,
                Interval = _interval,
                Enabled = true
            };
            _schedule.Elapsed += ScheduleElapsed;
        }

        public void Stop()
        {
            if (_schedule == null)
                return;

            _schedule.Close();
            _schedule.Dispose();
            _schedule = null;
        }

        private void ScheduleElapsed(object sender, ElapsedEventArgs e)
        {
            if (_interval <= 0)
            {
                _log.ErrorFormat("Interval is less or equals 0. Interval:{0}", _interval);
                return;
            }

            if (_executeDate != null)
            {
                if (DateTime.Now.AddMinutes(1) >= _executeDate.Value) // Add one minute in case timer launches a bit earlier
                {
                    LaunchEvent(this, e);
                }
                else
                {
                    _log.InfoFormat("Execute date in future. Now:{0}, ExecDate:{1}", DateTime.Now.ToString("o"), _executeDate.Value.ToString("o"));
                    SetExecuteDate(_executeDate.Value);
                }
            }
            else
            {
                LaunchEvent(this, e);
            }
        }

        public void SetExecuteDate(DateTime date)
        {
            _executeDate = date;

            var interval = (_executeDate.Value - DateTime.Now).TotalMilliseconds;
            _interval = interval > MaxInterval ? MaxInterval : interval;

            if (_schedule != null)
            {
                _schedule.Interval = _interval;
            }

            _log.InfoFormat("ExecuteDate:{0}, Interval:{1}", _executeDate, _interval);
        }

        private void LaunchEvent(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke(sender, e);
        }
    }
}
