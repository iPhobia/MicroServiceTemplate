using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Common;
using Common.Logger;
using Common.Repository;
using Common.ActionQueue;
using System.Configuration;

namespace WindowsService.Entity
{
    public class ActionQueueScheduler : IScheduler
    {
        private readonly BaseScheduler _scheduler;
        private readonly ActionFactory _actionFactory;
        private readonly BaseLogger _log;
        private readonly QueueRepository _repo;
        private bool _inProgress = false;


        public ActionQueueScheduler(BaseLogger log, QueueRepository repo)
        {
            _log = log;
            _repo = repo;
            _actionFactory = new ActionFactory(log, repo);
            _log.Log.Info("Configure Timer start");
            var minutes = Configuration.Configuration.Get.ActivationQueue.ActionQueueProcessingInterval;
            _scheduler = new BaseScheduler(1000 * 60 * minutes);
            _log.Log.InfoFormat("Configure Timer end. Interval {0} minutes", minutes);
        }

        public ActionQueueScheduler()
        {
            _actionFactory = new ActionFactory();
            _log.Log.Info("Configure Timer start");
            var minutes = 1;
            _scheduler = new BaseScheduler(1000 * 60 * minutes);
            _log.Log.InfoFormat("Configure Timer end. Interval {0} minutes", 1);
        }

        public void Run()
        {
            _log.Log.Info("Run timer");
            _scheduler.Run(ScheduleElapsed);
        }

        public void Stop()
        {
            _scheduler.Stop();
        }

        public void ScheduleElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ThreadContext.Properties["Thread"] = "ActionQueue";
                _log.Log.Info("Started");

                if (_inProgress)
                {
                    _log.Log.Info("InProgress true");
                    return;
                }

                _inProgress = true;

                var actionQueue = new ActionQueue(_actionFactory);
                actionQueue.ProcessQueue(_log.Log.Error, _log.Log.Info);

                _inProgress = false;
                _log.Log.Info("Finished");
            }
            catch (Exception ex)
            {
                _inProgress = false;
                _log.Log.ErrorFormat("ActionQueue crashed: {0}", ex.Message);
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
