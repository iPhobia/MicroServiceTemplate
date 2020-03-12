using log4net;
using Common.ActionQueue;

namespace Common.Logger
{
    public class BaseLogger : ILogger
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ILog Log
        {
            get
            {
                return _log;
            }
        }

        public void BeforeRecordProcessing(ActionQueueRecord record)
        {
            _log.InfoFormat("STARTED Process ActionQueueRecord. Id: {0}, TypeId: {1}", record.Id, record.ActionTypeId);
        }

        public void AfterRecordProcessing(ActionQueueRecord record)
        {
            _log.InfoFormat("FINISHED Process ActionQueueRecord. Id: {0}", record.Id);
        }
    }
}
