using Hwdtech;

namespace ShipNamespace
{
    public class StartCommand : IComand
    {
        private readonly Order order;

        public StartCommand(Order order)
        {
            this.order = order;
        }

        public void Execute()
        {
            order.Target.properties["Velocity"] = order.Velocity;
            var moveCommand = IoC.Resolve<ShipNamespace.IComand>("Operations.Move", order.Target);
            moveCommand = new InjectCommand(moveCommand);

            var targetQueue = (Queue<IComand>)order.Target.properties["CommandQueue"];
            targetQueue.Enqueue(moveCommand);
            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(moveCommand);
        }
    }
}
