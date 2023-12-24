namespace ShipNamespace
{
    public class StopMoveCommand : IComand
    {

        public void Execute()
        {

            foreach (var command in IoC.Resolve<Queue<IComand>>("Game.Queue"))
            {
                if (command is InjectCommand moveCommand)
                {
                    moveCommand.Inject(new EmptyCommand());
                    break;
                }
            }
        }
    }
}
