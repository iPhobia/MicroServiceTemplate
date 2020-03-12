using Common.Repository;

namespace Common.Action
{
    public interface IActionFactory
    {
        IRepository Repository { get; }
        IAction CreateAction(int typeId);
    }
}