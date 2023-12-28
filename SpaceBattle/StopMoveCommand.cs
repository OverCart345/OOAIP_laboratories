namespace ShipNamespace
{
    public class StopMoveCommand : IComand
    {
        private readonly IMoveStopOrder stopOrder;

        public StopMoveCommand(IMoveStopOrder stopOrder)
        {
            this.stopOrder = stopOrder;
        }

        public void Execute()
        {
            stopOrder.target.properties["Velocity"] = new Vector2d(0, 0);

            var targetQueue = (Queue<IComand>)stopOrder.target.properties["CommandQueue"];
            var commandToInject = (InjectCommand)targetQueue.Peek();
            commandToInject.Inject(new EmptyCommand());

        }
    }
}
