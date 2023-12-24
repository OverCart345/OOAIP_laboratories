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
            var movable = new MovableAdapter(order.Target);

            var moveCommand = new InjectCommand(new MoveCommand(movable));
            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(moveCommand);
        }
    }
}
