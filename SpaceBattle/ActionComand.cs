namespace ShipNamespace
{
    public class ActionCommand : IComand
    {
        private readonly Action action;
        public ActionCommand(Action action)
        {
            this.action = action;
        }

        public void Execute()
        {
            action();
        }
    }
}
