using Hwdtech;

namespace ShipNamespace
{
    public class LongOperationStrategy : IStrategy
    {
        private readonly string commandName;
        private readonly Order order;

        public LongOperationStrategy(string commandName, Order order)
        {
            this.commandName = commandName;
            this.order = order;
        }
        public object Strat(params object[] args)
        {
            var cmd = IoC.Resolve<IComand>("Config." + commandName, order.Target);
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands", (object[] args) => { return cmd; }).Execute();

            var startCmd = IoC.Resolve<IComand>("StartCommand", order);

            return startCmd;
        }
    }
}
