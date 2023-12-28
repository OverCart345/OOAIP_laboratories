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
            foreach (var property in order.properties)
            {
                order.Target.properties[property.Key] = order.properties[property.Key];
            }

            var moveCommand = IoC.Resolve<IComand>("Commands");
            var injectedMoveCommand = new InjectCommand(moveCommand);

            var targetQueue = (Queue<IComand>)order.Target.properties["CommandQueue"];
            targetQueue.Enqueue(injectedMoveCommand);
            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(moveCommand);
        }
    }
}
