using System;
using System.Collections.Generic;
using Common.ActionQueue;
using Common.Logger;
using Common.Entity;

namespace Common.Repository
{
    public class QueueRepository : CommonRepository, IRepository
    {
        private readonly BaseLogger _log;

        public QueueRepository()
        {
            _log = new BaseLogger();
            _log.Log.Debug("ctor");
        }

        public QueueRepository(BaseLogger logger)
        {
            _log = logger;
            _log.Log.Debug("ctor");
        }



        public IEnumerable<ActionQueueRecord> GetQueue()
        {
            return Query<ActionQueueRecord>("[dbo].[Test_GetListToProcess]");
        }

        public void SaveResponse(int queueId, GatewayResult result)
        {
            StatusUpdate(result.Description, result.StatudId, queueId);
        }

        public void StatusUpdateOnExeption(int queueId, ActionStatus statusId, string errorMessage)
        {
            Execute("[dbo].[Test_ActionStatusUpdateOnFailure]", new
            {
                errorMessage,
                queueId,
                statusId
            });
        }

        private void StatusUpdate(string statusDescription, ActionStatus actionStatusId, int queueId)
        {
            try
            {
                Execute("[dbo].[Test_ActionStatusUpdate]",
                        new
                        {
                            statusDescription,
                            queueId,
                            actionStatusId
                        });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
