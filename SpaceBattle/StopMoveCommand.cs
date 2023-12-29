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

            var command = (InjectCommand)stopOrder.target.properties["Command"];
            command.Inject(new EmptyCommand());
        }
    }
}
