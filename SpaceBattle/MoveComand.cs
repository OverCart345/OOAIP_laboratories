namespace ShipNamespace
{
    public class MoveCommand : IComand
    {
        private readonly IMovable movable;

        public MoveCommand(IMovable movable)
        {
            this.movable = movable;
        }

        public void Execute()
        {

            movable.Position = movable.Position + movable.Velocity;

        }
    }
}
