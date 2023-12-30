using Hwdtech;

namespace ShipNamespace
{
    public class LongOperationStrategy : IStrategy
    {
        public object Strat(params object[] args)
        {

            var command = IoC.Resolve<IComand>("CreateMacro", args[0]);
            var order = (Order)args[1];
            order.properties["Command"] = command;

            var startCmd = IoC.Resolve<IComand>("StartCommand", order);
            return startCmd;
        }
    }
}
