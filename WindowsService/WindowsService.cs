using System;
using System.Collections.Generic;
using System.ServiceProcess;
using WindowsService.Entity;
using Common.Logger;
using Common.Repository;
using Common;

namespace WindowsService
{
    public partial class WindowsService : ServiceBase
    {
        private static readonly BaseLogger log;
        private static readonly QueueRepository repo;
        private readonly List<IScheduler> _schedulers;

        static WindowsService()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                log = new BaseLogger();
                repo = new QueueRepository();
            }
            catch (Exception e)
            {
                log.Log.Error("Static constructor error: ", e);
                throw;
            }
        }

        public WindowsService()
        {
            try
            {
                InitializeComponent();
                _schedulers = new List<IScheduler>();
            }
            catch (Exception e)
            {
                log.Log.Error("Error in constructor", e);
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.Log.Info("Windows Service was started");

                _schedulers.Add(new ActionQueueScheduler(log, repo));
                StartJob();

                log.Log.Info("OnStart finished");
            }
            catch (Exception e)
            {
                log.Log.Fatal(e.Message, e);
            }
        }

        private void StartJob()
        {
            log.Log.Info("Started Job");
            _schedulers.ForEach(p => p.Run());
        }

        protected override void OnStop()
        {
            try
            {
                _schedulers.ForEach(p => p.Stop());
            }
            catch (Exception e)
            {
                log.Log.Error("OnStop error: ", e);
                throw;
            }
            log.Log.Info("Windows Service stopped");
        }
    }
}
