using System;
using System.Collections.Generic;
using Common.Action;

namespace Common.ActionQueue
{
    public class ActionQueue
    {
        private readonly IActionFactory _actionFactory;

        public ActionQueue(IActionFactory actionFactory)
        {
            _actionFactory = actionFactory;
        }

        public int ProcessQueue(Action<Exception> onError = null, Action<string> onSuccess = null)
        {
  
            IEnumerable<ActionQueueRecord> records = _actionFactory.Repository.GetQueue();
            onSuccess(string.Format("Found {0} records", Newtonsoft.Json.JsonConvert.SerializeObject(records)));
            
            return Process(records, onError, onSuccess);
        }

        private int Process(IEnumerable<ActionQueueRecord> records, Action<Exception> onError = null, Action<string> onSuccess = null)
        {
            var recordCounter = 0;
            try
            {
                foreach (var actionQueueRecord in records)
                {
                    try
                    {
                        onSuccess(string.Format("actionQueueRecord.Id {0}, actionQueueRecord.ActionTypeId {1}", actionQueueRecord.Id, actionQueueRecord.ActionTypeId));
                        if (actionQueueRecord != null)
                        {
                            IAction action = _actionFactory.CreateAction(actionQueueRecord.ActionTypeId);
                            action.Execute(actionQueueRecord.Id);

                            recordCounter++;
                            if (onSuccess != null)
                            {
                                onSuccess(string.Format("[Action {0}]: OK", actionQueueRecord.Id));
                            }
                            GC.Collect();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (onError != null)
                        {
                            onError(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (onError != null)
                {
                    onError(ex);
                }
            }

            return recordCounter;
        }
    }
}
