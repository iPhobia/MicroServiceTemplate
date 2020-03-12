using Common.ActionQueue;
using Common.Entity;
using Common.Logger;
using Common.Repository;
using System;

namespace Common.Action
{
    public abstract class BaseAction : IAction
    {
        protected readonly IRepository _repo;
        protected readonly ILogger _log;
        protected readonly string actionName;


        protected BaseAction(IRepository repo, ILogger logger, string action)
        {
            _repo = repo;
            _log = logger;
            actionName = action;
        }

        public virtual ActionResult Execute(int queueId)
        {
            try
            {
                LogStart();
                //throw new Exception("generated exeption");
                var gatewayResult = this.Run(queueId);
                _repo.SaveResponse(queueId, gatewayResult);

                LogFinish(gatewayResult);

                return new ActionResult
                {
                    Message = gatewayResult.Description
                };
            }
            catch (Exception ex)
            {
                LogException(ex);
                var errorMessage = ex.Message.Length > 255 ? ex.Message.Substring(0, 255) : ex.Message;
                _repo.StatusUpdateOnExeption(queueId, ActionStatus.Failed, errorMessage);

                return new ActionResult
                {
                    Message = errorMessage
                };
            }
        }

        protected abstract GatewayResult Run(int queueId);

        protected virtual void LogStart()
        {
            _log.Log.InfoFormat(actionName + " started");
        }

        protected virtual void LogFinish(GatewayResult gatewayResult)
        {
            _log.Log.InfoFormat(actionName + " finished. Result: Status Code:{0}  Description: {1}", gatewayResult.StatudId, gatewayResult.Description);
        }

        protected virtual void LogException(Exception ex)
        {
            _log.Log.Error(ex);
        }
    }
}
