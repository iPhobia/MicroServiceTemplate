using Common.ActionQueue;
using System.Collections.Generic;
using Common.Entity;

namespace Common.Repository
{
    public interface IRepository
    {
        IEnumerable<ActionQueueRecord> GetQueue();

        void SaveResponse(int queueId, GatewayResult result);
        void StatusUpdateOnExeption(int queueId, ActionStatus failed, string errorMessage);
    }
}