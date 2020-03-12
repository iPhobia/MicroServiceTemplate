using System;
using Common.Logger;
using Common.Repository;
using Common.ActionQueue;
using Common.Entity;

namespace Common.Action
{
    public class TestAction : BaseAction
    {
        public TestAction(IRepository repo, ILogger logger)
            : base(repo, logger, "TestAction")
        { }
       
        protected override GatewayResult Run(int queueId)
        {
            LogStart();

            var result = new GatewayResult
            {
                StatudId = ActionStatus.Processed,
                Description = "Succeeded"
            };

            LogFinish(result);

            return result;
        }
    }
}
