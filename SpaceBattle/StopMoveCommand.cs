using Hwdtech;

namespace ShipNamespace
{
    public class StopMoveCommand : IComand
    {

        public void Execute()
        {
            foreach (InjectCommand command in IoC.Resolve<Queue<IComand>>("Queue"))
            {
                command.Inject(new EmptyCommand());
                break;
            }
        }
    }
}
