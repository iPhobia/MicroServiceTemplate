using Common.Repository;
using Common.Logger;
using Common.Entity;
using Common.Action;

namespace Common.ActionQueue
{
    public class ActionFactory : IActionFactory
    {
        private QueueRepository _repo;
        private BaseLogger _log;

        public ActionFactory()
        {
            _repo = new QueueRepository();
            _log = new BaseLogger();
            _log.Log.Debug("ctor...");
        }

        public ActionFactory(BaseLogger logger, QueueRepository repository)
        {
            _repo = repository;
            _log = logger;
            _log.Log.Debug("ctor...");
        }

        public IRepository Repository
        {
            get
            {
                return _repo;
            }
        }

        public IAction CreateAction(int typeId)
        {
            IAction action = null;

            var actionType = (ActionType)typeId;
            switch(actionType)
            {
                case ActionType.Test:
                    action = new TestAction(_repo, _log);
                    break;
            }

            return action;
        }
    }
}
