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

            var command = (IComand)order.properties["Command"];

            var injectedMoveCommand = new InjectCommand(command);

            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(injectedMoveCommand);
        }
    }
}
